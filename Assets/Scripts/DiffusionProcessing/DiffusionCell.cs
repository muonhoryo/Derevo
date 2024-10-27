

using System.Linq;
using Derevo.Level;

namespace Derevo.DiffusionProcessing 
{
    public sealed class DiffusionCell
    {
        private DiffusionCell() { }
        public DiffusionCell(LevelCell Cell)
        {
            this.Cell = Cell;
        }

        private readonly LevelCell Cell;
        private DiffusionProcess CurrentDiffProc = null;

        public LevelCell Cell_
        {
            get => Cell;
        }

        public DiffusionProcess GetDiffusionProcess(int cellColumn,int cellRow)
        {
            if (CurrentDiffProc != null)
                return CurrentDiffProc;

            var parCell = Cell as ValuableCell;

            if (parCell == null)
                return null;

            var connectedCellsPoses = parCell.GetConnectedCellsPoses(new UnityEngine.Vector2Int(cellColumn,cellRow)).ToArray();

            if (connectedCellsPoses.Length == 0)
                return new DiffusionProcess(this);

            var connectedCells = connectedCellsPoses.Select((pos)=>DiffusionProcessing.GetDiffusionCell(pos.x,pos.y)).ToArray();

            DiffusionProcess GetProcess(int arrayIndex)
            {
                var connCellPos = connectedCellsPoses[arrayIndex];
                return connectedCells[arrayIndex].GetDiffusionProcess(connCellPos.x,connCellPos.y);
            }

            for(int i = 0; i < connectedCells.Length; i++)
            {
                var diffProc = GetProcess(i);
                if(CurrentDiffProc==null&&
                    diffProc != null)
                {
                    CurrentDiffProc= diffProc;
                }
                else if (CurrentDiffProc != diffProc)
                {
                    CurrentDiffProc.Aggregate(diffProc);
                }
            }
            if (CurrentDiffProc == null)
                CurrentDiffProc = new DiffusionProcess(this);
            CurrentDiffProc.BecameAggregateTargetEvent += AggregateProcessAction;
            return CurrentDiffProc;
        }
        private void AggregateProcessAction(DiffusionProcess newOwner)
        {
            CurrentDiffProc.BecameAggregateTargetEvent -= AggregateProcessAction;
            CurrentDiffProc = newOwner;
            CurrentDiffProc.BecameAggregateTargetEvent += AggregateProcessAction;
        }
    }
}