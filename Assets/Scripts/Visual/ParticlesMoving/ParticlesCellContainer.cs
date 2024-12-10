


using System;
using System.Collections.Generic;
using System.Linq;
using Derevo.Level;
using Derevo.PlayerControl;
using Derevo.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Derevo.Visual
{
    public sealed class ParticlesCellContainer : MonoBehaviour, ICellContainer
    {
        private sealed class ParticlesFermentationProcess
        {
            private ParticlesFermentationProcess() { }
            public ParticlesFermentationProcess(DiffusionParticle Owner)
            {
                this.Owner = Owner;
                PrevPos = Owner.transform.position;
            }

            public readonly DiffusionParticle Owner;
            public Vector2 PrevPos;
            public bool IsActiveFermentation = true;


            /// <summary>
            /// Return true if fermentation of particle is done
            /// </summary>
            /// <returns></returns>
            public bool CheckFermentationDone()
            {
                Vector2 step = (Vector2)Owner.transform.position - PrevPos;
                PrevPos = Owner.transform.position;
                if (step.magnitude <= GlobalConstsHandler.Instance_.ParticlesFermentationThreshold)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static int ParticlesSortFunc(ParticlesFermentationProcess x, ParticlesFermentationProcess y)
        {
            if (x.Owner.transform.position.y < y.Owner.transform.position.y)
                return -1;
            else if (x.Owner.transform.position.y > y.Owner.transform.position.y)
                return 1;
            else
            {
                if (x.Owner.transform.position.x < y.Owner.transform.position.x)
                    return -1;
                else if (x.Owner.transform.position.x > y.Owner.transform.position.x)
                    return 1;
                else
                    return 0;
            }
        }
        private static ParticlesFermentationProcess ParticlesSelectionFunc(DiffusionParticle owner) =>
            new ParticlesFermentationProcess(owner);
        private static DiffusionParticle ParticlesSelectionFunc(ParticlesFermentationProcess info) =>
            info.Owner;

        private List<ParticlesFermentationProcess> UploadedParticles=new List<ParticlesFermentationProcess>();
        public int UploadedParticlesCount_ =>UploadedParticles.Count;

        public Vector2Int CellPosition_ { get; private set; }
        public Vector2 UploadPosition_ => (Vector2)transform.position + GlobalConstsHandler.Instance_.CellsUploadPositionOffset;
        public Vector2 ExtractPosition_ => transform.position;

        private float RowsStart;


        public DiffusionParticle[] ExtractParticles(int extractedCount)
        {
            if (extractedCount > UploadedParticlesCount_)
                throw new System.Exception("Cannot extract " + extractedCount + " particles. Maximum count is " + UploadedParticlesCount_);

            DiffusionParticle[] exParts;
            if (extractedCount == UploadedParticlesCount_)
            {
                exParts = UploadedParticles.Select(ParticlesSelectionFunc).ToArray();
                UploadedParticles = new List<ParticlesFermentationProcess>();
            }
            else
            {
                UploadedParticles.Sort(ParticlesSortFunc);
                int newListLength = UploadedParticlesCount_ - extractedCount;
                exParts = UploadedParticles.Skip(newListLength).Select(ParticlesSelectionFunc).ToArray();
                UploadedParticles = UploadedParticles.Take(newListLength).ToList();
            }
            return exParts;
        }

        public void UploadParticles(DiffusionParticle[] uploadedParticles)
        {
            if (uploadedParticles == null || uploadedParticles.Length == 0)
                throw new ArgumentNullException("Missing particles array");

            foreach (var par in uploadedParticles)
                par.TurnMovingOn();
            UploadedParticles.AddRange(uploadedParticles.Select(ParticlesSelectionFunc));
        }

        public void UploadParticles(DiffusionParticle uploadedParticle)
        {
            if (uploadedParticle == null)
                throw new ArgumentNullException("Missing particle");

            uploadedParticle.TurnMovingOn();
            UploadedParticles.Add(new ParticlesFermentationProcess(uploadedParticle));
        }

        public void Initialize(Vector2Int cellPosition)
        {
            CellPosition_ = cellPosition;
            RowsStart = cellPosition.y - (Mathf.Sqrt(3) / 2) + GlobalConstsHandler.Instance_.ParticlesFixingRadius;
        }

        private void Update()
        {
            if (UploadedParticlesCount_ > 0)
            {
                foreach (var parInfo in UploadedParticles)
                {
                    if (parInfo.IsActiveFermentation && parInfo.CheckFermentationDone())
                    {
                        FixParticle(parInfo);
                    }
                }
            }
        }
        private void FixParticle(ParticlesFermentationProcess parInfo)
        {
            float particleRadius = GlobalConstsHandler.Instance_.ParticlesFixingRadius;
            Vector2 cellGlobalPos = transform.position;
            Vector2 parGlobalPos = parInfo.Owner.transform.position;
            //Vector2 centerOffset = new Vector2(cellGlobalPos.x % particleRadius, cellGlobalPos.y % particleRadius);
            Vector2 parLocalPos = parGlobalPos - cellGlobalPos;
            int row = Mathf.RoundToInt((parLocalPos.y-RowsStart) /CellsVisualManager.PhysicContainersRowHeight);
            bool isOdd = row % 2 == 0;
            int column = Mathf.RoundToInt((isOdd ? parLocalPos.x : parLocalPos.x - particleRadius) / (particleRadius * 2));
            Vector2 parNewPos = new Vector2(column * particleRadius * 2, row * CellsVisualManager.PhysicContainersRowHeight);
            parInfo.Owner.transform.position = parNewPos;
            parInfo.Owner.TurnMovingOff();
            parInfo.IsActiveFermentation = false;
        }
    }
}