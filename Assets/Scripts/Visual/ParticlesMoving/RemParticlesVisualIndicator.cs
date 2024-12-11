


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
        [SerializeField] private float BottomOffset;

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
        private List<FreeParticleMoving> FreeParticlesList=new List<FreeParticleMoving>();

        public Vector2 UploadPosition_ => UploadLocalPosition+(Vector2)transform.position;
        public Vector2 ExtractPosition_ => ExtractLocalPosition+(Vector2)transform.position;
        public int UploadedParticlesCount_ => FixedParticles.Count+FreeParticlesList.Count;

        private float BottomPosition;
        private int HighestRow=0;
        private float FixLevel=0;
        private Coroutine HighestRowCalculationCoroutine;
        private Coroutine ParticlesStopHandlingCoroutine;

        public DiffusionParticle[] ExtractParticles(int extractedCount)
        {
            if (extractedCount > UploadedParticlesCount_)
                throw new System.Exception("Cannot extract " + extractedCount + " particles. Maximum count is " + UploadedParticlesCount_);

            DiffusionParticle[] exParts;
            if(extractedCount== UploadedParticlesCount_)
            {
                exParts = FixedParticles.Concat(FreeParticlesList.Select(SelectParticlesFunc)).ToArray();
                FixedParticles = new List<DiffusionParticle>();
                FreeParticlesList=new List<FreeParticleMoving>();
            }
            else
            {
                if (extractedCount <= FreeParticlesList.Count)
                {
                    if (extractedCount == FreeParticlesList.Count)
                    {
                        exParts = FreeParticlesList.Select(SelectParticlesFunc).ToArray();
                        FreeParticlesList = new List<FreeParticleMoving>();
                    }
                    else
                    {
                        exParts = FreeParticlesList.TakeLast(extractedCount).Select(SelectParticlesFunc).ToArray();
                        FreeParticlesList = FreeParticlesList.SkipLast(extractedCount).ToList();
                    }
                }
                else
                {
                    int extractedCount_fixeParts = extractedCount - FreeParticlesList.Count;
                    exParts = FixedParticles.TakeLast(extractedCount_fixeParts).Concat(FreeParticlesList.Select(SelectParticlesFunc)).ToArray();
                    FreeParticlesList = new List<FreeParticleMoving>();
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
            int row = Mathf.RoundToInt((target.transform.position.y-BottomPosition) / CellsVisualManager.PhysicContainersRowHeight);
            float y = row * CellsVisualManager.PhysicContainersRowHeight+BottomPosition;
            float x = target.transform.position.x;
            bool isOdd = row % 2 == 0;
            if (isOdd)
                x += GlobalConstsHandler.Instance_.ParticlesFixingRadius;
            x = Mathf.RoundToInt(x / (GlobalConstsHandler.Instance_.ParticlesFixingRadius * 2));
            x *= GlobalConstsHandler.Instance_.ParticlesFixingRadius * 2;
            if (isOdd)
                x -= GlobalConstsHandler.Instance_.ParticlesFixingRadius;
            target.transform.position = new Vector2(x, y);
            target.TurnMovingOff();
        }
        private void FreeParticle(DiffusionParticle target) 
        {
            FreeParticlesList.Add(new FreeParticleMoving(target));
            target.TurnMovingOn();
        }
        private void FreeParticles(DiffusionParticle[] targets)
        {
            FreeParticlesList.AddRange(targets.Select(SelectParticleFunc));
            foreach (var par in targets)
                par.TurnMovingOn();
        }

        public void UploadParticles(DiffusionParticle[] uploadedParticles)
        {
            FreeParticles(uploadedParticles);
            StopCoroutine(HighestRowCalculationCoroutine);
            StartCoroutine(HighestRowCalculation());
        }

        public void UploadParticles(DiffusionParticle uploadedParticle)
        {
            FreeParticle(uploadedParticle);
            StopCoroutine(HighestRowCalculationCoroutine);
            StartCoroutine(HighestRowCalculation());
        }

        private void Awake()
        {
            BottomPosition = transform.position.y + BottomOffset;
            HighestRow = 0;
            FixLevel = BottomPosition;
        }
        private void Start()
        {
            HighestRowCalculationCoroutine = StartCoroutine(HighestRowCalculation());
            ParticlesStopHandlingCoroutine = StartCoroutine(ParticlesStopHandling());
        }
        private void Update()
        {
            FreeParticleMoving par;
            for(int i = 0; i < FreeParticlesList.Count; i++)
            {
                par = FreeParticlesList[i];
                if (par.Owner.transform.position.y >= FixLevel)
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
                RecalculateHighestRow();
            }
        }
        private void RecalculateHighestRow()
        {
            if (UploadedParticlesCount_ == 0)
                return;
            float highestHeight = float.MinValue;
            foreach (var par in FreeParticlesList)
            {
                if (par.Owner.transform.position.y > highestHeight)
                    highestHeight = par.Owner.transform.position.y;
            }
            foreach (var par in FixedParticles)
            {
                if (par.transform.position.y > highestHeight)
                    highestHeight = par.transform.position.y;
            }
            int newHighestRow = Mathf.RoundToInt((highestHeight-BottomPosition) / CellsVisualManager.PhysicContainersRowHeight);
            if (newHighestRow != HighestRow)
            {
                int oldHighestRow = HighestRow;
                HighestRow = newHighestRow;
                FixLevel = (HighestRow-GlobalConstsHandler.Instance_.RemParticlesVisualIndicator_FreeRowsCount) * CellsVisualManager.PhysicContainersRowHeight+BottomPosition;
                if (oldHighestRow > HighestRow)
                {
                    RefillFreeParticlesList();
                }
            }
        }
        private IEnumerator ParticlesStopHandling()
        {
            while (true)
            {
                yield return new WaitForSeconds(GlobalConstsHandler.Instance_.ParticlesContainers_StopHandlingTime);
                FreeParticleMoving par;
                for(int i = 0; i < FreeParticlesList.Count; i++)
                {
                    par = FreeParticlesList[i];
                    if (par.Owner.transform.position.y < FixLevel)
                    {
                        if (par.CheckStopping())
                        {
                            FreeParticlesList.RemoveAt(i);
                            FixParticle(par.Owner);
                            i--;
                        }
                        else
                            par.UpdatePrevPos();
                    }
                }
            }
        }
    }
}