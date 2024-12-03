

using System.Linq;
using Derevo.Level;
using UnityEngine;

namespace Derevo.DiffusionProcessing 
{
    public sealed class DiffusionCell
    {
        private DiffusionCell() { }
        public DiffusionCell(LevelCell Cell,Vector2Int CellPosition)
        {
            this.Cell = Cell;
            this.CellPosition= CellPosition;
        }

        private readonly LevelCell Cell;
        private readonly Vector2Int CellPosition;
        private DiffusionProcess CurrentDiffProc = null;

        public LevelCell Cell_ => Cell;
        public Vector2Int CellPosition_ => CellPosition;

        public DiffusionProcess GetDiffusionProcess(int cellColumn,int cellRow)
        {
            return InternalGetDiffusionProcess(cellColumn, cellRow, new DiffusionProcess(this));
        }
        private DiffusionProcess InternalGetDiffusionProcess(int cellColumn,int cellRow,in DiffusionProcess defaultProc)
        {
            if (CurrentDiffProc != null)
                return CurrentDiffProc;

            var parCell = Cell as ValuableCell;

            if (parCell == null)
                return null;

            var connectedCellsPoses = parCell.GetConnectedCellsPoses(new Vector2Int(cellColumn, cellRow)).ToArray();

            if (connectedCellsPoses.Length == 0)
                return new DiffusionProcess(this);

            var connectedCells = connectedCellsPoses.Select((pos) => DiffusionProcessing.GetDiffusionCell(pos.x, pos.y)).ToArray();

            DiffusionProcess GetProcess(int arrayIndex)
            {
                var connCellPos = connectedCellsPoses[arrayIndex];
                return connectedCells[arrayIndex].InternalGetDiffusionProcess(connCellPos.x, connCellPos.y,CurrentDiffProc);
            }

            CurrentDiffProc = defaultProc;
            bool isInitializeProc = false;

            for (int i = 0; i < connectedCells.Length; i++)
            {
                var diffProc = GetProcess(i);
                if (!isInitializeProc)
                {
                    if (diffProc != CurrentDiffProc)
                    {
                        CurrentDiffProc = diffProc;
                    }
                    isInitializeProc= true;
                    CurrentDiffProc.AddMember(this);
                }
                else if(CurrentDiffProc!=diffProc)
                {
                    CurrentDiffProc.Aggregate(diffProc);
                }
            }
            if (!isInitializeProc)
                CurrentDiffProc.AddMember(this);

            CurrentDiffProc.BecameAggregateTargetEvent += AggregateProcessAction;
            return CurrentDiffProc;
        }

        private void AggregateProcessAction(DiffusionProcess newOwner)
        {
            if(CurrentDiffProc!=null)
                CurrentDiffProc.BecameAggregateTargetEvent -= AggregateProcessAction;
            CurrentDiffProc = newOwner;
            CurrentDiffProc.BecameAggregateTargetEvent += AggregateProcessAction;
        }
    }
}