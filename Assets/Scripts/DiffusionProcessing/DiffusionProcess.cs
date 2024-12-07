
using System;
using System.Collections.Generic;
using UnityEngine;
using Derevo.Level;

namespace Derevo.DiffusionProcessing 
{
    public sealed class DiffusionProcess
    {
        public event Action<DiffusionProcess> BecameAggregateTargetEvent = delegate { };

        private DiffusionProcess() { }
        public DiffusionProcess(DiffusionCell owner, params DiffusionCell[] members)
        {
            Members = new List<DiffusionCell>(1 + members.Length);
            Members.Add(owner);
            foreach(var member in members) 
            {
                Members.Add(member);
            }
        }

        private List<DiffusionCell> Members;

        public void AddMember(DiffusionCell member)
        {
            if(!Members.Contains(member))
                Members.Add(member);
        }
        public void Diffuse()
        {
            int sum = 0;
            int onlyValue;

            foreach(var member in Members)
            {
                sum += (member.Cell_ as ValuableCell).Value_;
            }
            int rem = sum % Members.Count;
            if (rem!=0)
            {
                sum -= rem;
                onlyValue = sum / Members.Count;
                int i = 0;
                for (; rem > 0; i++)
                {
                    LevelManager.TrySetCellValue(onlyValue + 1, Members[i].CellPosition_.x, Members[i].CellPosition_.y);
                    rem--;
                }
                for (; i < Members.Count; i++)
                {
                    LevelManager.TrySetCellValue(onlyValue, Members[i].CellPosition_.x, Members[i].CellPosition_.y);
                }
            }
            else
            {
                onlyValue = sum / Members.Count;
                foreach (var member in Members)
                {
                    LevelManager.TrySetCellValue(onlyValue, member.CellPosition_.x, member.CellPosition_.y);
                }
            }
        }
        public void Aggregate(DiffusionProcess otherProcess)
        {
            Members.AddRange(otherProcess.Members);
            otherProcess.BecameAggregateTarget(this);
        }
        public DiffusionCell[] GetMembers()
        {
            var cells=new DiffusionCell[Members.Count];
            Members.CopyTo(cells);
            return cells;
        }
        public Vector2[] GetDiffusionPath(Vector2Int origin,Vector2Int destination)
        {
            if (origin == destination)
                throw new ArgumentException("Origin cannot be equal destinanation.");

            int originIndex=-1;
            int destinationIndex=-1;
            Vector2[] path;
            for(int i = 0; i < Members.Count; i++)
            {
                if (Members[i].CellPosition_ == origin)
                {
                    originIndex = i;
                    break;
                }
            }
            for(int i = 0; i < Members.Count; i++)
            {
                if (Members[i].CellPosition_==destination)
                {
                    destinationIndex = i;
                    break;
                }
            }

            if (originIndex == -1)
                throw new ArgumentException("Cannot find cell with position: " + origin);
            if (destinationIndex == -1)
                throw new ArgumentException("Cannot find cell with position: " + destination);

            if (destinationIndex > originIndex)
            {
                path = new Vector2[destinationIndex - originIndex];
                for(int i = originIndex,j=0; j < path.Length; i++,j++)
                {
                    path[j] = Members[i].CellPosition_;
                }
            }
            else
            {
                path = new Vector2[originIndex - destinationIndex];
                for (int i = destinationIndex, j = 0; j < path.Length; i++, j++)
                {
                    path[j] = Members[i].CellPosition_;
                }
            }
            return path;
        }

        private void BecameAggregateTarget(DiffusionProcess aggregateOwner)
        {
            BecameAggregateTargetEvent(aggregateOwner);
        }
    }
}