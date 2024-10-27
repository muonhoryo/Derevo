
using System;
using System.Collections.Generic;
using Derevo.DiffusionProcessing;
using UnityEngine;

namespace Derevo.Level
{
    public class ValuableCell:LevelCell
    {
        public ValuableCell() { }
        public ValuableCell(int Value_,DiffusionDirection DiffusionDirection_)
        {
            this.Value_= Value_;
            this.DiffusionDirection_ = DiffusionDirection_;
        }
        public enum DiffusionDirection:ushort
        {
            Right=1,
            TopRight=2,
            Top=4,
            TopLeft=8,
            Left=16,
            BottomLeft=32,
            Bottom=64,
            BottomRight=128
        }

        private int Value=0;
        private DiffusionDirection DiffDirection=0;

        public int Value_ 
        {
            get=>Value;
            set
            {
                if (value < 0)
                    return;

                Value = value;
            } 
        }
        public DiffusionDirection DiffusionDirection_
        {
            get=>DiffDirection;
            set
            {
                if ((ushort)value > 255)
                    return;

                DiffDirection = value;
            }
        }

        public virtual Vector2Int[] GetConnectedCellsPoses(Vector2Int cellPos)
        {
            return GetCellsFromDirection(DiffDirection, cellPos);
        }

        protected Vector2Int[] GetCellsFromDirection(DiffusionDirection direction,Vector2Int cellPos)
        {
            int subdirectionsCount = 0;
            for(int i=0;i<8; i++)
            {
                if (((int)direction & (1 << i)) != 0)
                    subdirectionsCount++;
            }
            if (subdirectionsCount == 0)
                return new Vector2Int[0];
            var cells = new List<Vector2Int>(subdirectionsCount);


            bool IsDirection(int digitCount) =>
                ((int)direction & (1 << digitCount))!= 0;

            bool CheckCell(Vector2Int pos)
            {
                var cell = LevelManager.GetCell(pos.x, pos.y);
                if (cell == null)
                    return false;
                return cell is ValuableCell;
            }

            bool TryAddCellPos(Vector2Int pos)
            {
                if (CheckCell(pos))
                {
                    cells.Add(pos);
                    if (cells.Count == cells.Capacity)
                        return true;
                }
                return false;
            }

            Vector2Int GetRightPos() =>
                new Vector2Int(cellPos.x + 1, cellPos.y);
            Vector2Int GetTopRightPos() =>
                new Vector2Int(cellPos.x + 1, cellPos.y + 1);
            Vector2Int GetTopPos() =>
                new Vector2Int(cellPos.x, cellPos.y + 1);
            Vector2Int GetTopLeftPos() =>
                new Vector2Int(cellPos.x - 1, cellPos.y + 1);
            Vector2Int GetLeftPos() =>
                new Vector2Int(cellPos.x - 1, cellPos.y);
            Vector2Int GetBottomLeftPos() =>
                new Vector2Int(cellPos.x - 1, cellPos.y - 1);
            Vector2Int GetBottomPos() =>
                new Vector2Int(cellPos.x, cellPos.y - 1);
            Vector2Int GetBottomRightPos() =>
                new Vector2Int(cellPos.x + 1, cellPos.y - 1);


            if (IsDirection(0))
            {
                if (TryAddCellPos(GetRightPos()))
                    return cells.ToArray();
            }
            if (IsDirection(1))
            {
                if (TryAddCellPos(GetTopRightPos()))
                    return cells.ToArray();
            }
            if (IsDirection(2))
            {
                if (TryAddCellPos(GetTopPos()))
                    return cells.ToArray();
            }
            if (IsDirection(3))
            {
                if (TryAddCellPos(GetTopLeftPos()))
                    return cells.ToArray();
            }
            if (IsDirection(4))
            {
                if(TryAddCellPos(GetLeftPos()))
                    return cells.ToArray();
            }
            if (IsDirection(5))
            {
                if (TryAddCellPos(GetBottomLeftPos()))
                    return cells.ToArray();
            }
            if(IsDirection(6))
            {
                if (TryAddCellPos(GetBottomPos()))
                    return cells.ToArray();
            }
            if (IsDirection(7))
            {
                if (TryAddCellPos(GetBottomRightPos()))
                    return cells.ToArray();
            }

            return cells.ToArray();
        }
    }
}