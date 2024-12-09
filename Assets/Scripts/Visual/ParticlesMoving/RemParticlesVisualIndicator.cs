


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Derevo.Visual
{
    public sealed class RemParticlesVisualIndicator : MonoBehaviour, IParticlesContainer
    {
        [SerializeField] private Vector2 UploadPosition;
        [SerializeField] private Vector2 ExtractPosition;
        [SerializeField] private int ParticlesSetWidth;
        [SerializeField] private bool IsSameRowsLengths;

        private List<DiffusionParticle> FixedParticles = new List<DiffusionParticle>();
        private List<DiffusionParticle> FreeParticles=new List<DiffusionParticle>();

        public Vector2 UploadPosition_ => UploadPosition;
        public Vector2 ExtractPosition_ => ExtractPosition;
        public int UploadedParticlesCount_ => FixedParticles.Count+FreeParticles.Count;

        public DiffusionParticle[] ExtractParticles(int extractedCount)
        {
            if (extractedCount > UploadedParticlesCount_)
                throw new System.Exception("Cannot extract " + extractedCount + " particles. Maximum count is " + UploadedParticlesCount_);

            DiffusionParticle[] exParts;
            if(extractedCount== UploadedParticlesCount_)
            {
                exParts = FixedParticles.Concat(FreeParticles).ToArray();
                FixedParticles = new List<DiffusionParticle>();
                FreeParticles=new List<DiffusionParticle>();
            }
            else
            {
                if (extractedCount <= FreeParticles.Count)
                {
                    if (extractedCount == FreeParticles.Count)
                    {
                        exParts = FreeParticles.ToArray();
                        FreeParticles = new List<DiffusionParticle>();
                    }
                    else
                    {
                        exParts = FreeParticles.TakeLast(extractedCount).ToArray();
                        FreeParticles = FreeParticles.SkipLast(extractedCount).ToList();
                    }
                }
                else
                {
                    int extractedCount_fixeParts = extractedCount - FreeParticles.Count;
                    exParts = FixedParticles.TakeLast(extractedCount_fixeParts).Concat(FreeParticles).ToArray();
                    FreeParticles = new List<DiffusionParticle>();
                    FixedParticles = FixedParticles.SkipLast(extractedCount_fixeParts).ToList();
                }
            }
            RefillFreeParticlesList();
            return exParts;
        }
        private void RefillFreeParticlesList()
        {
            int uploadedRows = Mathf.CeilToInt((float)UploadedParticlesCount_ / ParticlesSetWidth);
            int addParts = uploadedRows / 2; //Added to free particles list from fixed list parts
            if (!IsSameRowsLengths)
            {
                uploadedRows = Mathf.CeilToInt((float)UploadedParticlesCount_+addParts / ParticlesSetWidth);
            }
            int requariedFixedCount=(uploadedRows-GlobalConstsHandler.Instance_.RemParticlesVisualIndicator_FreeRowsCount)*ParticlesSetWidth;
            if (!IsSameRowsLengths)
            {
                requariedFixedCount -= addParts;
            }
            if (FixedParticles.Count == requariedFixedCount)
                return;

            if (FixedParticles.Count > requariedFixedCount) //Need to free some particles
            {
                int reqToFreeCount = FixedParticles.Count - requariedFixedCount;
                IEnumerable<DiffusionParticle> parts = FixedParticles.TakeLast(reqToFreeCount);
                foreach (var p in parts)
                    p.TurnMovingOn();
                FreeParticles.AddRange(parts);
            }
            else
            {

            }
        }
        private void FixParticle(DiffusionParticle target)
        {

        }

        public void UploadParticles(DiffusionParticle[] uploadedParticles)
        {
            throw new System.NotImplementedException();
        }

        public void UploadParticles(DiffusionParticle uploadedParticle)
        {
            throw new System.NotImplementedException();
        }
    }
}