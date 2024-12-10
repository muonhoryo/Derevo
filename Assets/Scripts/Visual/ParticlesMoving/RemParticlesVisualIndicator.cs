


using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Derevo.Serialization;
using Derevo.UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using MuonhoryoLibrary.Unity;

namespace Derevo.Visual
{
    public sealed class RemParticlesVisualIndicator : MonoBehaviour, IParticlesContainer
    {
        [SerializeField] private Vector2 UploadLocalPosition;
        [SerializeField] private Vector2 ExtractLocalPosition;

        private static DiffusionParticle SelectParticlesFunc(FreeParticleMoving par) =>
            par.Owner;
        private static FreeParticleMoving SelectParticleFunc(DiffusionParticle par) =>
            new FreeParticleMoving(par);
        private sealed class FreeParticleMoving
        {
            private FreeParticleMoving() { }
            public FreeParticleMoving(DiffusionParticle Owner)
            {
                this.Owner = Owner;
                PrevPos = Owner.transform.position;
            }

            public readonly DiffusionParticle Owner;
            public Vector2 PrevPos;

            public bool CheckStopping()
            {
                if (Mathf.Abs(((Vector2)Owner.transform.position - PrevPos).magnitude) <= GlobalConstsHandler.Instance_.ParticlesFermentationThreshold)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public void UpdatePrevPos()
            {
                PrevPos = Owner.transform.position;
            }
        }

        private List<DiffusionParticle> FixedParticles = new List<DiffusionParticle>();
        private List<FreeParticleMoving> FreeParticles=new List<FreeParticleMoving>();

        public Vector2 UploadPosition_ => UploadLocalPosition+(Vector2)transform.position;
        public Vector2 ExtractPosition_ => ExtractLocalPosition+(Vector2)transform.position;
        public int UploadedParticlesCount_ => FixedParticles.Count+FreeParticles.Count;

        private RectTransform RectTransform;
        private int HighestRow;
        private float FixLevel;
        private Coroutine HighestRowCalculationCoroutine;

        public DiffusionParticle[] ExtractParticles(int extractedCount)
        {
            if (extractedCount > UploadedParticlesCount_)
                throw new System.Exception("Cannot extract " + extractedCount + " particles. Maximum count is " + UploadedParticlesCount_);

            DiffusionParticle[] exParts;
            if(extractedCount== UploadedParticlesCount_)
            {
                exParts = FixedParticles.Concat(FreeParticles.Select(SelectParticlesFunc)).ToArray();
                FixedParticles = new List<DiffusionParticle>();
                FreeParticles=new List<FreeParticleMoving>();
            }
            else
            {
                if (extractedCount <= FreeParticles.Count)
                {
                    if (extractedCount == FreeParticles.Count)
                    {
                        exParts = FreeParticles.Select(SelectParticlesFunc).ToArray();
                        FreeParticles = new List<FreeParticleMoving>();
                    }
                    else
                    {
                        exParts = FreeParticles.TakeLast(extractedCount).Select(SelectParticlesFunc).ToArray();
                        FreeParticles = FreeParticles.SkipLast(extractedCount).ToList();
                    }
                }
                else
                {
                    int extractedCount_fixeParts = extractedCount - FreeParticles.Count;
                    exParts = FixedParticles.TakeLast(extractedCount_fixeParts).Concat(FreeParticles.Select(SelectParticlesFunc)).ToArray();
                    FreeParticles = new List<FreeParticleMoving>();
                    FixedParticles = FixedParticles.SkipLast(extractedCount_fixeParts).ToList();
                }
                RefillFreeParticlesList();
            }
            return exParts;
        }
        private void RefillFreeParticlesList()
        {
            List<DiffusionParticle> newFixedList = new List<DiffusionParticle>();
            foreach(var par in FixedParticles)
            {
                if (par.transform.position.y < FixLevel)
                {
                    newFixedList.Add(par);
                }
                else
                {
                    FreeParticle(par);
                }
            }
            FixedParticles = newFixedList;
        }
        private void FixParticle(DiffusionParticle target)
        {
            FixedParticles.Add(target);
            int row = Mathf.RoundToInt(target.transform.position.y / CellsVisualManager.PhysicContainersRowHeight);
            float y = row * CellsVisualManager.PhysicContainersRowHeight;
            float x = Mathf.RoundToInt(target.transform.position.x / GlobalConstsHandler.Instance_.ParticlesFixingRadius * 2);
            if (row % 2 == 0)
                x += GlobalConstsHandler.Instance_.ParticlesFixingRadius;
            target.transform.position = new Vector2(x, y);
            target.TurnMovingOff();
        }
        private void FreeParticle(DiffusionParticle target) 
        {
            FreeParticles.Add(new FreeParticleMoving(target));
            target.TurnMovingOn();
        }

        public void UploadParticles(DiffusionParticle[] uploadedParticles)
        {
            FreeParticles.AddRange(uploadedParticles.Select(SelectParticleFunc));
            StopCoroutine(HighestRowCalculationCoroutine);
            StartCoroutine(HighestRowCalculation());
        }

        public void UploadParticles(DiffusionParticle uploadedParticle)
        {
            FreeParticles.Add(new FreeParticleMoving(uploadedParticle));
            StopCoroutine(HighestRowCalculationCoroutine);
            StartCoroutine(HighestRowCalculation());
        }

        private void Awake()
        {
            RectTransform = transform as RectTransform;
        }
        private void Start()
        {
            HighestRowCalculationCoroutine = StartCoroutine(HighestRowCalculation());
        }
        private void Update()
        {
            FreeParticleMoving par;
            for(int i = 0; i < FreeParticles.Count; i++)
            {
                par = FreeParticles[i];
                if (par.Owner.transform.position.y < FixLevel)
                {
                    if (par.CheckStopping())
                    {
                        FreeParticles.RemoveAt(i);
                        FixParticle(par.Owner);
                        i--;
                    }
                    else
                        par.UpdatePrevPos();
                }
                else
                {
                    float forceLevel = Random.Range(GlobalConstsHandler.Instance_.RemParticlesVisualIndicator_MinForce,
                        GlobalConstsHandler.Instance_.RemParticlesVisualIndicator_MaxForce);
                    float angle = Random.Range(0, 360);
                    par.Owner.AddForce(angle.DirectionOfAngle() * forceLevel);
                    par.UpdatePrevPos();
                }
            }
        }
        private IEnumerator HighestRowCalculation()
        {
            while (true)
            {
                yield return new WaitForSeconds(GlobalConstsHandler.Instance_.RemParticlesVisualIndicator_HighestRowCalculationDelay);
                if (UploadedParticlesCount_==0)
                    continue;
                float highestHeight = float.MinValue;
                foreach (var par in FreeParticles)
                {
                    if (par.Owner.transform.position.y > highestHeight)
                        highestHeight = par.Owner.transform.position.y;
                }
                foreach(var par in FixedParticles)
                {
                    if (par.transform.position.y > highestHeight)
                        highestHeight = par.transform.position.y;
                }
                int newHighestRow = Mathf.RoundToInt(highestHeight/ CellsVisualManager.PhysicContainersRowHeight);
                if (newHighestRow != HighestRow)
                {
                    int oldHighestRow = HighestRow;
                    HighestRow = newHighestRow;
                    FixLevel = HighestRow * CellsVisualManager.PhysicContainersRowHeight;
                    if (oldHighestRow > HighestRow)
                    {
                        RefillFreeParticlesList();
                    }
                }
            }
        }
    }
}