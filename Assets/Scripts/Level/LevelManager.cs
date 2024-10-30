
using Derevo.DiffusionProcessing;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derevo.Level
{
    public static partial class LevelManager 
    {
        public struct ChangeCellValueEventInfo
        {
            public ValuableCell ChangedCell;
            public int OldValue;
            public int NewValue;
            public int Column;
            public int Row;

            public ChangeCellValueEventInfo(ValuableCell changedCell, int oldValue, int newValue, int column, int row)
            {
                ChangedCell = changedCell;
                OldValue = oldValue;
                NewValue = newValue;
                Column = column;
                Row = row;
            }
        }
        public struct ChangeCellDiffDIrectionEventInfo
        {
            public ValuableCell ChangedCell;
            public ValuableCell.DiffusionDirection OldDirection;
            public ValuableCell.DiffusionDirection NewDirection;
            public int Column;
            public int Row;

            public ChangeCellDiffDIrectionEventInfo(ValuableCell changedCell, ValuableCell.DiffusionDirection oldDirection, ValuableCell.DiffusionDirection newDirection, int column, int row)
            {
                ChangedCell = changedCell;
                OldDirection = oldDirection;
                NewDirection = newDirection;
                Column = column;
                Row = row;
            }
        }
        public struct ChangeCellTypeValueEventInfo
        {
            public LevelCell OldCell;
            public LevelCell NewCell;
            public int Column;
            public int Row;

            public ChangeCellTypeValueEventInfo(LevelCell oldCell, LevelCell newCell, int column, int row)
            {
                OldCell = oldCell;
                NewCell = newCell;
                Column = column;
                Row = row;
            }
        }

        public static event Action<ChangeCellValueEventInfo> ChangeCellValueEvent = delegate { };
        public static event Action<ChangeCellDiffDIrectionEventInfo> ChangeCellDiffDirectionEvent = delegate { };
        public static event Action<ChangeCellTypeValueEventInfo> ChangeCellTypeEvent = delegate { };
        public static event Action InitializeMapEvent = delegate { };

        private static LevelCell[][] LevelMap;
        private static int MaxHeight;
        private static bool IsFirstCellBottom = true;

        public static int Width_ => LevelMap.Length;
        public static int MaxHeight_ => MaxHeight;
        public static bool IsFirstCellBottom_ => IsFirstCellBottom;

        public static DiffusionCell[][] GetDiffusionMap()
        {
            DiffusionCell[][] diffusionMap = new DiffusionCell[Width_][];

            for (int i = 0; i < Width_; i++)
            {
                diffusionMap[i] = new DiffusionCell[LevelMap[i].Length];
                for (int j = 0; j < LevelMap[i].Length; j++)
                {
                    diffusionMap[i][j] = new DiffusionCell(LevelMap[i][j], new Vector2Int(i, j));
                }
            }
            return diffusionMap;
        }
        public static DiffusionProcess[] InitializeDiffusionProcesses()
        {
            var procs = new List<DiffusionProcess>();
            void SubscribeOnAggEvent(DiffusionProcess owner)
            {
                void BecameAggTargetAction(DiffusionProcess newProc)
                {
                    owner.BecameAggregateTargetEvent -= BecameAggTargetAction;
                    procs.Remove(owner);
                    SubscribeOnAggEvent(newProc);
                }
                owner.BecameAggregateTargetEvent += BecameAggTargetAction;
            }

            ValuableCell parCell;
            Vector2Int pos;
            for (int i = 0; i < Width_; i++)
            {
                for (int j = 0; j < LevelMap[i].Length; j++)
                {
                    parCell = DiffusionProcessing.DiffusionProcessing.LastStartedProcessInfo_.DiffusionMap[i][j].Cell_ as ValuableCell;
                    if (parCell != null)
                    {
                        pos = new Vector2Int(i, j);
                        if (parCell.HasConnection(pos))
                        {
                            var process = DiffusionProcessing.DiffusionProcessing.LastStartedProcessInfo_.DiffusionMap[i][j].GetDiffusionProcess(i, j);
                            if (process != null)
                            {
                                SubscribeOnAggEvent(process);
                                procs.Add(process);
                            }
                        }
                    }
                }
            }
            return procs.ToArray();
        }
        public static int?[][] GetValueMap()
        {
            int?[][] valueMap = new int?[Width_][];
            ValuableCell cell;
            for (int i = 0; i < Width_; i++)
            {
                valueMap[i] = new int?[LevelMap[i].Length];
                for(int j=0;j<LevelMap[i].Length; j++)
                {
                    cell = LevelMap[i][j] as ValuableCell;
                    if (cell != null)
                        valueMap[i][j] = cell.Value_;
                }
            }
            return valueMap;
        }
        public static LevelCell GetCell(int column,int row)
        {
            if (!CheckCellPos(column,row))
                return null;

            return LevelMap[column][row];
        }
        public static void SetCellValue(int value,int column,int row)
        {
            if (!CheckCellPos(column, row))
                return;

            var parsCell= LevelMap[column][row] as ValuableCell;
            if (parsCell != null)
            {
                InternalSetCellValue(parsCell, value, column, row);
            }
        }
        public static void SetCellDiffusionDirection(ValuableCell.DiffusionDirection direction,int column,int row)
        {
            if (!CheckCellPos(column, row))
                return;

            var parsCell = LevelMap[column][row] as ValuableCell;
            if (parsCell != null)
            {
                InternalSetDiffusionDirection(parsCell, direction, column, row);
            }
        }
        public static void ResetValuableCellsDirections()
        {
            for(int i = 0; i < Width_; i++)
            {
                for(int j = 0; j < LevelMap[i].Length; j++)
                {
                    var parsCell = LevelMap[i][j] as ValuableCell;
                    if (parsCell != null)
                    {
                        InternalSetDiffusionDirection(parsCell,0, i, j);
                    }
                }
            }
        }
        public static void SetCellType(LevelCell newCell,int column,int row)
        {
            if (!CheckCellPos(column, row))
                return;
            var oldCell = LevelMap[column][row];
            LevelMap[column][row] = newCell;
            newCell.Clone(oldCell);
            var info = new ChangeCellTypeValueEventInfo(oldCell, newCell, column, row);
            ChangeCellTypeEvent(info);
        }
        public static LevelMapInfo GetLevelMapInfo()
        {
            throw new Exception("MRE");
        }
        public static void InitializeLevel(LevelMapInfo info)
        {
            if (!ValidateLevelMapInfo(info))
                throw new Exception("Invalid LevelMapInfo.");

            IsFirstCellBottom = info.IsFirstCellBottom;
            LevelMap = new LevelCell[info.CellsInfo.Length][];
            Func<LevelCellInfo, LevelCell> selectFunc = (cellInfo) =>
                cellInfo.InitializeCell();
            for(int i = 0; i < Width_; i++)
            {
                LevelMap[i] = info.CellsInfo[i].Select(selectFunc).ToArray();
                if (LevelMap[i].Length>MaxHeight)
                    MaxHeight = LevelMap[i].Length;
            }
            InitializeMapEvent();
        }

        public static bool CheckCellPos(int column,int row)
        {
            return column >= 0 && column < Width_ &&
                row >= 0 && row < LevelMap[column].Length;
        }

        private static void InternalSetCellValue(ValuableCell target, int value, int column, int row)
        {
            int oldValue = target.Value_;
            target.Value_ = value;
            if (oldValue == target.Value_)
                return;

            var info = new ChangeCellValueEventInfo(target, oldValue, value, column, row);
            ChangeCellValueEvent(info);
        }
        private static void InternalSetDiffusionDirection(ValuableCell target, ValuableCell.DiffusionDirection direction, int column, int row)
        {
            ValuableCell.DiffusionDirection oldValue = target.DiffusionDirection_;
            if(!target.TrySetDiffusionDirection(direction, column, row))
            {
                return;
            }

            var info = new ChangeCellDiffDIrectionEventInfo(target, oldValue, direction, column, row);
            ChangeCellDiffDirectionEvent(info);
        }
        private static bool ValidateLevelMapInfo(LevelMapInfo info)
        {
            if (info.CellsInfo==null)
                return false;
            foreach (var column in info.CellsInfo)
            {
                if (column==null)
                    return false;
                foreach(var cell in column)
                {
                    if (cell == null)
                        return false;
                }
            }
            return true;
        }
    }
}