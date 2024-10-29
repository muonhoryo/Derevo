
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
        public enum DiffusionDirection : ushort
        {
            Right = 1,
            TopRight = 2,
            Top = 4,
            TopLeft = 8,
            Left = 16,
            BottomLeft = 32,
            Bottom = 64,
            BottomRight = 128
        }
        protected Vector2Int GetPositionFromDirectionByDigit(int digit, Vector2Int cellPos)
        {
            switch (digit)
            {
                case 0:
                    return new Vector2Int(cellPos.x + 1, cellPos.y);
                case 1:
                    return new Vector2Int(cellPos.x + 1, cellPos.y + 1);
                case 2:
                    return new Vector2Int(cellPos.x, cellPos.y + 1);
                case 3:
                    return new Vector2Int(cellPos.x - 1, cellPos.y + 1);
                case 4:
                    return new Vector2Int(cellPos.x - 1, cellPos.y);
                case 5:
                    return new Vector2Int(cellPos.x - 1, cellPos.y - 1);
                case 6:
                    return new Vector2Int(cellPos.x, cellPos.y - 1);
                case 7:
                    return new Vector2Int(cellPos.x + 1, cellPos.y - 1);
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
        public void SetDiffusionDirection(DiffusionDirection direction, int column, int row)
        {
            SetDiffusionDirectionField(ref DiffDirection, direction, column, row);
        }

        protected Vector2Int[] GetCellsFromDirection(DiffusionDirection direction, Vector2Int cellPos)
        {
            int subdirectionsCount = 0;
            for (int i = 0; i < 8; i++)
            {
                if (((int)direction & (1 << i)) != 0)
                    subdirectionsCount++;
            }
            if (subdirectionsCount == 0)
                return new Vector2Int[0];
            var cells = new List<Vector2Int>(subdirectionsCount);

            bool TryAddCellPos(int digit)
            {
                if(TryGetPositionFromDirectionByDigit(direction,digit,cellPos,out Vector2Int newPos)&&
                    CheckCellConnection(newPos))
                {
                    cells.Add(newPos);
                    if (cells.Count == cells.Capacity)
                        return true;
                }
                return false;
            }

            for(int i = 0; i < 8; i++)
            {
                if (TryAddCellPos(i))
                    return cells.ToArray();
            }

            return cells.ToArray();
        }
        protected bool HasConnectionByDirection(DiffusionDirection direction, Vector2Int cellPos)
        {
            bool CheckDirection(int digitCount, Vector2Int connectionPos)
            {
                return ((int)direction & (1 << digitCount)) != 0 && CheckCellConnection(connectionPos);
            }

            return CheckDirection(0, GetRightPos(cellPos)) ||
                CheckDirection(1, GetTopRightPos(cellPos)) ||
                CheckDirection(2, GetTopPos(cellPos)) ||
                CheckDirection(3, GetTopLeftPos(cellPos)) ||
                CheckDirection(4, GetLeftPos(cellPos)) ||
                CheckDirection(5, GetBottomLeftPos(cellPos)) ||
                CheckDirection(6, GetBottomPos(cellPos)) ||
                CheckDirection(7, GetBottomRightPos(cellPos));
        }
        protected void SetDiffusionDirectionField(ref DiffusionDirection directionField, DiffusionDirection newDirection, int column, int row)
        {
            if ((int)newDirection > 255)
                return;

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