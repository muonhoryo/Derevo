

using System.Linq;
using UnityEngine;

namespace Derevo.Level
{
    public sealed class ExtenderCell : ValuableCell
    {
        public ExtenderCell(int Value_,DiffusionDirection DiffusionDirection_,DiffusionDirection ExtendDirection_):base(Value_, DiffusionDirection_)
        {
            this.ExtendDirection_ = ExtendDirection_;
        }
        private DiffusionDirection ExtendDirection=0;
        public DiffusionDirection ExtendDirection_
        {
            get => ExtendDirection;
            set
            {
                if ((ushort)value> MaxDIffusionDirectionValue)
                    return;

                ExtendDirection = value;
            }
        }

        public bool TrySetExtendDirection(DiffusionDirection direction,int column,int row)
        {
            return TrySetDiffusionDirectionField(ref ExtendDirection, direction, column, row);
        }

        public override Vector2Int[] GetConnectedCellsPoses(Vector2Int cellPos)
        {
            Vector2Int[] mainConn =base.GetConnectedCellsPoses(cellPos);
            if (mainConn.Length > 0)
            {
                return mainConn.Union(GetCellsFromDirection(ExtendDirection, cellPos)).ToArray();
            }
            else
            {
                return GetCellsFromDirection(ExtendDirection, cellPos);
            }
        }
        public override bool HasConnection(Vector2Int cellPos)
        {
            if (base.HasConnection(cellPos))
                return true;
            else
            {
                return HasConnectionByDirection(ExtendDirection, cellPos);
            }
        }

        public override void Clone(LevelCell source)
        {
            base.Clone(source);
            if(source is ExtenderCell parsSource)
            {
                ExtendDirection=parsSource.ExtendDirection_;
            }
        }
    }
}