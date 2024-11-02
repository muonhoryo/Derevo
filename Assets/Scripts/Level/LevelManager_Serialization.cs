

using System;
using UnityEngine;

namespace Derevo.Level
{
    public static partial class LevelManager
    {
        [Serializable]
        public struct LevelMapInfo
        {
            public LevelMapColumnInfo[] ColumnsInfo;
            public bool IsFirstCellBottom;
            public int ParticlesCount;

            public static LevelMapInfo ConvertLevelMapToSerializationInfo(in LevelCell[][] mapInfo)
            {
                LevelMapInfo info=new LevelMapInfo();
                info.ColumnsInfo=new LevelMapColumnInfo[mapInfo.Length];
                for(int i = 0; i < info.ColumnsInfo.Length; i++)
                {
                    info.ColumnsInfo[i].CellsInfo = new LevelMapCellInfo[mapInfo[i].Length];
                    for (int j=0;j< mapInfo[i].Length; j++)
                    {
                        info.ColumnsInfo[i].CellsInfo[j] = LevelMapCellInfo.GetLevelCellInfo(mapInfo[i][j]);
                    }
                }
                return info;
            }
        }
        [Serializable]
        public struct LevelMapColumnInfo
        {
            public LevelMapColumnInfo(LevelMapCellInfo[] CellsInfo)
            {
                this.CellsInfo= CellsInfo;
            }

            public LevelMapCellInfo[] CellsInfo;

            public static LevelMapColumnInfo[] ConvertMapInfoToColumnsInfo(in LevelMapCellInfo[][] cellInfo)
            {
                LevelMapColumnInfo[] columnsInfo=new LevelMapColumnInfo[cellInfo.Length];
                for(int i = 0; i < cellInfo.Length; i++)
                {
                    columnsInfo[i] = new LevelMapColumnInfo();
                    columnsInfo[i].CellsInfo = cellInfo[i];
                }
                return columnsInfo;
            }
        }

        [Serializable]
        public struct LevelMapCellInfo
        {
            public LevelMapCellInfo(bool IsValuable,int Value,ushort ExtendDirection)
            {
                this.IsValuable = IsValuable;
                this.Value = Value;
                this.ExtendDirection= ExtendDirection;
            }
            public bool IsValuable;
            public int Value;
            public ushort ExtendDirection;

            public static LevelCell InitializeCell(LevelMapCellInfo cellInfo)
            {
                int GetValidatedValue()
                {
                    return cellInfo.Value>=0?cellInfo.Value : 0;
                }
                ValuableCell.DiffusionDirection GetValidatedDirection()
                {
                    return cellInfo.ExtendDirection <= ValuableCell.MaxDIffusionDirectionValue ? (ValuableCell.DiffusionDirection)cellInfo.ExtendDirection : ValuableCell.DefaultDirection;
                }

                if (!cellInfo.IsValuable)
                {
                    return new BlockCell();
                }
                else if (cellInfo.ExtendDirection == 0)
                {
                    return new ValuableCell(GetValidatedValue(), 0);
                }
                else
                {
                    return new ExtenderCell(GetValidatedValue(), 0, GetValidatedDirection());
                }
            }
            public static LevelMapCellInfo GetLevelCellInfo(LevelCell cell)
            {
                if(cell is ExtenderCell exCel)
                    return new LevelMapCellInfo(true, exCel.Value_, (ushort)exCel.ExtendDirection_);
                else if(cell is ValuableCell valCel)
                    return new LevelMapCellInfo(true, valCel.Value_, 0);
                else
                    return new LevelMapCellInfo(false, 0, 0);
                    
            }
        }
    }
}