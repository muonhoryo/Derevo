
using System;
using System.Collections.Generic;
using System.Text;
using Derevo.DiffusionProcessing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Derevo.Level
{
    public class ValuableCell : LevelCell
    {
        public ValuableCell() { }
        public ValuableCell(int Value, DiffusionDirection DiffDirection)
        {
            this.Value = Value;
            this.DiffDirection = DiffDirection;
        }
        public const int MaxDiffusionDirectionDigitsCount = 6;
        public const int MaxDiffusionDirectionValue = 64;
        public const DiffusionDirection DefaultDirection = DiffusionDirection.Top;
        public enum DiffusionDirection : ushort
        {
            TopRight=1,
            Top=2,
            TopLeft=4,
            BottomLeft=8,
            Bottom=16,
            BottomRight=32,
            CannotHaveDirections=64
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

        public int Value_ => Value;
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
        public bool TrySetValue(int value)
        {
            if (value < 0||value==Value)
                return false;
            else
            {
                Value = value;
                return true;
            }
        }
        public void SetDefaultDirection(Vector2Int cellPos)
        {
            bool result = TrySetDiffusionDirection(DefaultDirection, cellPos.x, cellPos.y);
            if (!result)
            {
                for(int i = 0; i < MaxDiffusionDirectionDigitsCount; i++)
                {
                    DiffusionDirection newDir = (DiffusionDirection)(1 << i);
                    if (newDir == DefaultDirection)
                        continue;
                    if (TrySetDiffusionDirection(newDir, cellPos.x, cellPos.y))
                        return;
                }
                TrySetDiffusionDirection(DiffusionDirection.CannotHaveDirections,cellPos.x, cellPos.y);
            }
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
            int parNewDir = (int)newDirection;
            if (parNewDir> MaxDiffusionDirectionValue)
                return false;
            if (newDirection != DiffusionDirection.CannotHaveDirections&&
                newDirection!=0)
            {
                Vector2Int cellPos = new Vector2Int(column, row);
                Vector2Int connPos;
                int mask = 1;
                for (int i = 0; i < 8; i++)
                {
                    if (TryGetPositionFromDirectionByDigit(newDirection, i, cellPos, out connPos)) //In direction mask this direction is written
                    {
                        //if (i == 1)
                        //{
                        //    void PrintDigits(int value)
                        //    {
                        //        StringBuilder str = new StringBuilder(32);
                        //        for(int i = 0; i < 32; i++)
                        //        {
                        //            str.Append((value & 1 << i) == 0 ? 0 : 1);
                        //        }
                        //        Debug.Log(str.ToString());
                        //    }
                        //}

                        if (!CheckCellConnection(connPos))
                            newDirection = (DiffusionDirection)((int)newDirection & ~(mask << i));
                    }
                }
                if (newDirection == 0) //Result haven't any options to set new direction so return false
                {
                    return false;
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

        public static float GetRotationValueFromDirection(DiffusionDirection direction)
        {
            switch (direction)
            {
                case DiffusionDirection.TopRight:
                    return 30;
                case DiffusionDirection.Top:
                    return 90;
                case DiffusionDirection.TopLeft:
                    return 150;
                case DiffusionDirection.BottomLeft:
                    return 210;
                case DiffusionDirection.Bottom:
                    return 270;
                case DiffusionDirection.BottomRight:
                    return 330;
                default:
                    return 0;
            }
        }
    }
   
}