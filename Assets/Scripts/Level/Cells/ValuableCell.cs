
using System;
using System.Collections.Generic;
using Derevo.DiffusionProcessing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Derevo.Level
{
    public class ValuableCell : LevelCell
    {
        public ValuableCell() { }
        public ValuableCell(int Value_, DiffusionDirection DiffDirection)
        {
            this.Value_ = Value_;
            this.DiffDirection = DiffDirection;
        }
        public const int MaxDiffusionDirectionDigitsCount = 6;
        public const int MaxDIffusionDirectionValue = 63;
        public const DiffusionDirection DefaultDirection = DiffusionDirection.Top;
        public enum DiffusionDirection : ushort
        {
            TopRight=1,
            Top=2,
            TopLeft=4,
            BottomLeft=8,
            Bottom=16,
            BottomRight=32
        }
        protected Vector2Int GetPositionFromDirectionByDigit(int digit, Vector2Int cellPos)
        {
            Vector2Int GetRight() =>
                new Vector2Int(cellPos.x + 1, cellPos.y);
            Vector2Int GetTopRight() =>
                new Vector2Int(cellPos.x + 1, cellPos.y + 1);
            Vector2Int GetTop() =>
                new Vector2Int(cellPos.x, cellPos.y + 1);
            Vector2Int GetLeft() =>
                new Vector2Int(cellPos.x - 1, cellPos.y);
            Vector2Int GetTopLeft() =>
                new Vector2Int(cellPos.x - 1, cellPos.y + 1);
            Vector2Int GetBottomLeft() =>
                new Vector2Int(cellPos.x - 1, cellPos.y - 1);
            Vector2Int GetBottom() =>
                new Vector2Int(cellPos.x, cellPos.y - 1);
            Vector2Int GetBottomRight() =>
                new Vector2Int(cellPos.x + 1, cellPos.y - 1);


            switch (digit)
            {
                case 0:
                    return (LevelManager.IsFirstCellBottom_ == (cellPos.x % 2 == 0)) ? GetRight() : GetTopRight();
                case 1:
                    return GetTop();
                case 2:
                    return (LevelManager.IsFirstCellBottom_ == (cellPos.x % 2 == 0)) ? GetLeft() : GetTopLeft();
                case 3:
                    return (LevelManager.IsFirstCellBottom_ != (cellPos.x % 2 == 0)) ? GetLeft() : GetBottomLeft();
                case 4:
                    return GetBottom();
                case 5:
                    return (LevelManager.IsFirstCellBottom_ != (cellPos.x % 2 == 0)) ? GetRight() : GetBottomRight();
                default:
                    return -Vector2Int.one;
            }
        }
        protected bool TryGetPositionFromDirectionByDigit(DiffusionDirection direction,int digit,Vector2Int cellPos, out Vector2Int result)
        {
            if(((int)direction & (1 << digit)) != 0)
            {
                result = GetPositionFromDirectionByDigit(digit, cellPos);
                return true;
            }
            else
            {
                result = -Vector2Int.one;
                return false;
            }
        }
        private int Value = 0;
        private DiffusionDirection DiffDirection = 0;

        public int Value_
        {
            get => Value;
            set
            {
                if (value < 0)
                    return;

                Value = value;
            }
        }
        public DiffusionDirection DiffusionDirection_ => DiffDirection;

        public virtual Vector2Int[] GetConnectedCellsPoses(Vector2Int cellPos)
        {
            return GetCellsFromDirection(DiffDirection, cellPos);
        }
        public virtual bool HasConnection(Vector2Int cellPos)
        {
            return HasConnectionByDirection(DiffDirection, cellPos);
        }
        public bool TrySetDiffusionDirection(DiffusionDirection direction, int column, int row)
        {
            return TrySetDiffusionDirectionField(ref DiffDirection, direction, column, row);
        }

        protected Vector2Int[] GetCellsFromDirection(DiffusionDirection direction, Vector2Int cellPos)
        {
            if(direction==0)
                return new Vector2Int[0];

            var cells = new List<Vector2Int>();

            for(int i = 0; i < 8; i++)
            {
                if (TryGetPositionFromDirectionByDigit(direction, i, cellPos, out Vector2Int newPos) &&
                    CheckCellConnection(newPos))
                {
                    cells.Add(newPos);
                }
            }

            return cells.ToArray();
        }
        protected bool HasConnectionByDirection(DiffusionDirection direction, Vector2Int cellPos)
        {
            if (direction == 0)
                return false;

            Vector2Int result;
            for(int i = 0; i < 8; i++)
            {
                if (TryGetPositionFromDirectionByDigit(direction, i, cellPos, out result))
                {
                    return true;
                }
            }
            return false;
        }
        protected bool TrySetDiffusionDirectionField(ref DiffusionDirection directionField, DiffusionDirection newDirection, int column, int row)
        {
            if ((int)newDirection > MaxDIffusionDirectionValue)
                return false;

            Vector2Int cellPos = new Vector2Int(column, row);
            Vector2Int connPos;
            int mask =  - 1;
            for(int i = 0; i < 8; i++)
            {
                if(TryGetPositionFromDirectionByDigit(newDirection,i,cellPos,out connPos))
                {
                    if (!CheckCellConnection(connPos))
                        newDirection =(DiffusionDirection)((int)newDirection & (mask << i));
                }
            }
            if (directionField != newDirection)
            {
                directionField = newDirection;
                return true;
            }
            return false;
        }

        private bool CheckCellConnection(Vector2Int targetPos)
        {
            var cell = LevelManager.GetCell(targetPos.x, targetPos.y);
            if (cell == null)
                return false;
            return cell is ValuableCell;
        }

        public override void Clone(LevelCell source)
        {
            if (source is ValuableCell parsSource)
            {
                Value = parsSource.Value;
                DiffDirection = parsSource.DiffDirection;
            }
        }
    }
   
}