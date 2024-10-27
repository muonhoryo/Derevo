
using System;
using System.Collections.Generic;
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
            Members.Add(member);
        }
        public void Diffuse()
        {
            throw new Exception("MRE");
        }
        public void Aggregate(DiffusionProcess otherProcess)
        {
            Members.AddRange(otherProcess.Members);
            otherProcess.BecameAggregateTarget(this);
        }

        private void BecameAggregateTarget(DiffusionProcess aggregateOwner)
        {
            BecameAggregateTargetEvent(aggregateOwner);
        }
    }
}