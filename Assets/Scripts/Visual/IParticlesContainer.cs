

using UnityEngine;

namespace Derevo.Visual
{
    public interface IParticlesContainer
    {
        public DiffusionParticle[] ExtractParticles(int extractedCount);
        public void UploadParticles(DiffusionParticle[] uploadedParticles);
        public void UploadParticles(DiffusionParticle uploadedParticle);

        public Vector2 UploadPosition_ { get; }
        public Vector2 ExtractPosition_ { get; }
        public int UploadedParticlesCount_ { get; }
    }
    public interface ICellContainer:IParticlesContainer
    {
        public Vector2Int CellPosition_ { get; }
    }
}