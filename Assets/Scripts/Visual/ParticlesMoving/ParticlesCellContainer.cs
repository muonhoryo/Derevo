


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Derevo.UI;
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
                    IsActiveFermentation = false;
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
        private Coroutine UploadDelayCoroutine;
        private Coroutine ParticlesStopHandlingCoroutine;


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
                throw new ArgumentNullException("Particles array");

            foreach (var par in uploadedParticles)
            {
                DisperseParticle(par);
                par.TurnMovingOn();
            }
            UploadedParticles.AddRange(uploadedParticles.Select(ParticlesSelectionFunc));
            StartUploadDelayCoroutine();
        }
        public void UploadParticles(DiffusionParticle uploadedParticle)
        {
            if (uploadedParticle == null)
                throw new ArgumentNullException("Missing particle");

            DisperseParticle(uploadedParticle);
            uploadedParticle.TurnMovingOn();
            UploadedParticles.Add(new ParticlesFermentationProcess(uploadedParticle));
            StartUploadDelayCoroutine();
        }
        private void DisperseParticle(DiffusionParticle particle)
        {
            float min = -GlobalConstsHandler.Instance_.ParticleCellContainer_UploadXPosDispersion;
            float max = GlobalConstsHandler.Instance_.ParticleCellContainer_UploadXPosDispersion;
            particle.transform.position = new Vector3(
                particle.transform.position.x + UnityEngine.Random.Range(min, max),
                particle.transform.position.y,
                particle.transform.position.z);
        }
        private void StartUploadDelayCoroutine()
        {
            if (UploadDelayCoroutine != null)
                StopCoroutine(UploadDelayCoroutine);
            UploadDelayCoroutine = StartCoroutine(UploadDelay());
        }
        private IEnumerator UploadDelay()
        {
            if(ParticlesStopHandlingCoroutine!=null)
                StopCoroutine(ParticlesStopHandlingCoroutine);
            yield return new WaitForSeconds(GlobalConstsHandler.Instance_.ParticlesCellContainer_UploadDelay);
            ParticlesStopHandlingCoroutine = StartCoroutine(ParticlesStopHandling());
        }

        public void Initialize(Vector2Int cellPosition)
        {
            CellPosition_ = cellPosition;
            RowsStart = transform.position.y + GlobalConstsHandler.Instance_.ParticlesCellContainer_BottomOffset;
        }

        private void Start()
        {
            ParticlesStopHandlingCoroutine= StartCoroutine(ParticlesStopHandling());
        }
        private IEnumerator ParticlesStopHandling()
        {
            while (true)
            {
                yield return new WaitForSeconds(GlobalConstsHandler.Instance_.ParticlesContainers_StopHandlingTime);
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
        }
        private void FixParticle(ParticlesFermentationProcess parInfo)
        {
            float particleRadius = GlobalConstsHandler.Instance_.ParticlesFixingRadius;
            Vector2 parGlobalPos = parInfo.Owner.transform.position;
            int row;
            int column;
            row = Mathf.RoundToInt((parGlobalPos.y-RowsStart) / CellsVisualManager.PhysicContainersRowHeight);
            bool isOdd = row % 2 == 0;
            column = Mathf.RoundToInt((isOdd ? parGlobalPos.x : parGlobalPos.x - particleRadius) / (particleRadius * 2));
            Vector2 parNewPos = new Vector2(isOdd?column * particleRadius * 2:column*particleRadius*2-particleRadius, row * CellsVisualManager.PhysicContainersRowHeight+RowsStart);
            parInfo.Owner.transform.position = parNewPos;
            parInfo.Owner.TurnMovingOff();
            parInfo.IsActiveFermentation = false;
        }
    }
}