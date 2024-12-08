


using System.Collections.Generic;
using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;

namespace Derevo.Visual
{
    public sealed class ParticlesCellContainer : MonoBehaviour, ICellContainer
    {
        private List<DiffusionParticle> UploadedParticles=new List<DiffusionParticle>();
        public int UploadedParticlesCount_ =>UploadedParticles.Count;

        public Vector2Int CellPosition_ { get; private set; }

        public Vector2 UploadPosition_ => (Vector2)transform.position + GlobalConstsHandler.Instance_.CellsUploadPositionOffset;

        public Vector2 ExtractPosition_ => transform.position;


        public DiffusionParticle[] ExtractParticles(int extractedCount)
        {
            if (extractedCount > UploadedParticlesCount_)
                throw new System.Exception("Cannot extract " + extractedCount + " particles. Maximum count is " + UploadedParticlesCount_);


        }

        public void UploadParticles(DiffusionParticle[] uploadedParticles)
        {
            throw new System.NotImplementedException();
        }

        public void UploadParticles(DiffusionParticle uploadedParticle)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(Vector2Int cellPosition)
        {
            CellPosition_ = cellPosition;
        }
    }
}