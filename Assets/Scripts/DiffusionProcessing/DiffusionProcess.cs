
using System;
using Derevo.Level;

namespace Derevo.DiffusionProcessing 
{
    public sealed class DiffusionProcess
    {
        private DiffusionProcess() { }
        public DiffusionProcess(ValuableCell owner, params ValuableCell[] members)
        {
            throw new Exception("MRE");
        }

        public void AdddMember(ValuableCell member)
        {
            throw new Exception("MRE");
        }
        public void Diffuse()
        {
            throw new Exception("MRE");
        }
        public void Aggregate(DiffusionProcess otherProcess)
        {
            throw new Exception("MRE");
        }
    }
}