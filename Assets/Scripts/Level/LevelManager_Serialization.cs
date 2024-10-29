

using System;

namespace Derevo.Level
{
    public static partial class LevelManager
    {
        [Serializable]
        public struct LevelMapInfo
        {
            public int Width;
            public int Height;
            public LevelCellInfo[][] CellsInfo; 
        }

        public abstract class LevelCellInfo 
        {
            public abstract LevelCell InitializeCell();
        }
        [Serializable]
        public sealed class BlockCellInfo : LevelCellInfo
        {
            public override LevelCell InitializeCell()
            {
                return new BlockCell();
            }
        }
        [Serializable]
        public class ValuableCellInfo : LevelCellInfo
        {
            public ValuableCellInfo(int Value) 
            {
                this.Value = Value;
            }

            public readonly int Value;

            public override LevelCell InitializeCell()
            {
                return new ValuableCell(Value, 0);
            }
        }
        [Serializable]
        public sealed class ExtenderCellInfo:ValuableCellInfo
        {
            public ExtenderCellInfo(int Value,ValuableCell.DiffusionDirection ExtendDirection):base(Value)
            {
                this.ExtendDirection=ExtendDirection;
            }

            public readonly ValuableCell.DiffusionDirection ExtendDirection;

            public override LevelCell InitializeCell()
            {
                return new ExtenderCell(Value, 0, ExtendDirection);
            }
        }
    }
}