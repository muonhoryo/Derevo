

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
                if ((ushort)value> 255)
                    return;

                ExtendDirection = value;
            }
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
                return mainConn;
            }
        }
    }
}