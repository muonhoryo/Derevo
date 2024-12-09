


using Derevo.DiffusionProcessing;
using UnityEngine;
using MuonhoryoLibrary.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Derevo.Visual
{
    public sealed class DiffParticlesMovingManager : MonoBehaviour
    {
        private static DiffParticlesMovingManager Instance;

        public static void MoveCell2Cell(DiffusionProcess diffProc, ICellContainer origin, ICellContainer destination,
            float[] particlesSpeeds, int particlesCount)
        {
            Instance.MoveCell2Cell_NoSt(diffProc,origin,destination, particlesSpeeds, particlesCount);
        }
        public static void MoveDirectly(IParticlesContainer origin, IParticlesContainer destination, float[] particlesSpeeds, int particlesCount)
        {
            Instance.MoveDirectly_NoSt(origin,destination,particlesSpeeds,particlesCount);
        }
        public static void MoveDirectly(DiffusionParticle[] particles,IParticlesContainer destination, float[] particlesSpeeds)
        {
            Instance.MoveDirectly_NoSt(particles,destination,particlesSpeeds);
        }

        private readonly struct ParticlesMoving
        {
            public sealed class ParticleInfo
            {
                public readonly DiffusionParticle Owner;
                public int CurrentTargetIndex=0;
                public readonly float MovingSpeed;

                public ParticleInfo(DiffusionParticle owner, float movingSpeed)
                {
                    Owner = owner;
                    MovingSpeed = movingSpeed;
                }
            }

            public readonly ParticleInfo[] Particles;
            public readonly Vector2[] Path;
            public readonly IParticlesContainer Destination;
            public readonly IParticlesContainer Origin;

            public ParticlesMoving(DiffusionParticle[] particles,float[] particlesSpeeds, Vector2[] path, IParticlesContainer destination, IParticlesContainer origin)
            {
                Particles = new ParticleInfo[particles.Length];
                for(int i = 0; i < particles.Length; i++)
                {
                    Particles[i] = new ParticleInfo(particles[i], particlesSpeeds[i]);
                }
                Path = path;
                Destination = destination;
                Origin = origin;
            }

            /// <summary>
            /// Return true if moving of every particle has done.
            /// </summary>
            /// <returns></returns>
            public readonly bool Move()
            {
                Vector3 dir;
                ParticleInfo curInfo;
                bool hasNotMovable = true;
                for(int i = 0; i < Particles.Length; i++)
                {
                    curInfo = Particles[i];
                    if (curInfo.CurrentTargetIndex <= Path.Length)
                    {
                        hasNotMovable = false;
                        dir = Path[curInfo.CurrentTargetIndex] - (Vector2)curInfo.Owner.transform.position;
                        if (dir.magnitude <= curInfo.MovingSpeed)
                        {
                            curInfo.Owner.transform.position = Path[curInfo.CurrentTargetIndex];
                            curInfo.CurrentTargetIndex++;
                            if (curInfo.CurrentTargetIndex > Path.Length)
                                Destination.UploadParticles(curInfo.Owner);
                        }
                        else
                        {
                            dir = dir.normalized;
                            curInfo.Owner.transform.position += dir * curInfo.MovingSpeed;
                        }
                    }
                }
                return hasNotMovable;
            }
            public readonly int GetMovedParticlesCount()
            {
                int count = 0;
                foreach(var par in Particles)
                {
                    if(par.CurrentTargetIndex>Path.Length) 
                        count++;
                }
                return count;
            }
        }

        private readonly SingleLinkedList<ParticlesMoving> MovingList = new SingleLinkedList<ParticlesMoving> { };
        [SerializeField] private GameObject ParticlesParent;

        public void MoveCell2Cell_NoSt(DiffusionProcess diffProc, ICellContainer origin, ICellContainer destination,
            float[] particlesSpeeds, int particlesCount)
        {
            if (diffProc == null)
                throw new ArgumentNullException("DiffusionProccess cannot be null.");
            if (origin == null)
                throw new ArgumentNullException("Origin cannot be null.");
            if (destination == null)
                throw new ArgumentNullException("Destination cannot be null.");
            if (origin == destination)
                throw new ArgumentException("Origin cannot be equal destination.");
            if (particlesCount <= 0)
                throw new ArgumentException("Particles count must be more than 0");
            if (particlesSpeeds == null|| particlesSpeeds.Length!=particlesCount)
                throw new ArgumentNullException("Missing speeds");

            Vector2[] path= diffProc.GetDiffusionPath(origin.CellPosition_, destination.CellPosition_);
            path=path.Concat(new Vector2[] { destination.UploadPosition_}).ToArray();
            DiffusionParticle[] particles= origin.ExtractParticles(particlesCount);
            ParticlesMoving info = new ParticlesMoving(particles, particlesSpeeds,path,destination,origin);
            InternalStartMove(info);
        }
        public void MoveDirectly_NoSt(IParticlesContainer origin,IParticlesContainer destination, float[] particlesSpeeds,int particlesCount)
        {
            if (origin == null)
                throw new ArgumentNullException("Origin cannot be null.");
            if (destination == null)
                throw new ArgumentNullException("Destination cannot be null.");
            if (origin == destination)
                throw new ArgumentException("Origin cannot be equal destination.");
            if (particlesCount <= 0)
                throw new ArgumentException("Particles count must be more than 0");
            if (particlesSpeeds == null || particlesSpeeds.Length != particlesCount)
                throw new ArgumentNullException("Missing speeds");

            while (true)
            {
                if (CheckInterruption(origin, out ParticlesMoving? interrProc))
                {
                    ParticlesMoving parIntProc = (ParticlesMoving)interrProc;
                    int movedParCount = parIntProc.GetMovedParticlesCount();
                    Func<ParticlesMoving.ParticleInfo, bool> filterFunc = (par) => par.CurrentTargetIndex <= parIntProc.Path.Length;
                    Func<ParticlesMoving.ParticleInfo, DiffusionParticle> particlesSelectionFunc = (par) => par.Owner;
                    Func<ParticlesMoving.ParticleInfo, float> speedsSelectionFunc = (par) => par.MovingSpeed;

                    IEnumerable<ParticlesMoving.ParticleInfo> freeParticles= parIntProc.Particles.Where(filterFunc);
                    IEnumerable<DiffusionParticle> freeParParticles = freeParticles.Select(particlesSelectionFunc);
                    if (movedParCount >= particlesCount)
                    {
                        MoveDirectly_NoSt(freeParParticles.Take(particlesCount).ToArray(),destination, particlesSpeeds);
                        if (movedParCount > particlesCount)
                        {
                            float[] newSpeeds = freeParticles.Select(speedsSelectionFunc).Skip(particlesCount).ToArray();

                            MoveDirectly_NoSt(freeParParticles.Skip(particlesCount).ToArray(), destination, newSpeeds);
                        }
                        break;
                    }
                    else
                    {
                        MoveDirectly_NoSt(freeParParticles.ToArray(), destination, particlesSpeeds.Take(movedParCount).ToArray());
                        particlesCount -= movedParCount;
                        particlesSpeeds = particlesSpeeds.Skip(movedParCount).ToArray();
                    }
                    MovingList.Remove(parIntProc);
                }
                else
                    break;
            }

            Vector2[] path = new Vector2[]
            {
                origin.ExtractPosition_,
                destination.UploadPosition_
            };
            DiffusionParticle[] particles = origin.ExtractParticles(particlesCount);
            ParticlesMoving info = new ParticlesMoving(particles,particlesSpeeds,path,destination,origin);
            InternalStartMove(info);
        }
        public void MoveDirectly_NoSt(DiffusionParticle[] particles,IParticlesContainer destination, float[] particlesSpeeds)
        {
            if (destination == null)
                throw new ArgumentNullException("Destination cannot be null.");
            if (particlesSpeeds == null || particlesSpeeds.Length != particles.Length)
                throw new ArgumentNullException("Missing speeds");

            Vector2[] path = new Vector2[] { destination.UploadPosition_ };
            ParticlesMoving info = new ParticlesMoving(particles,particlesSpeeds,path,destination,null);
            InternalStartMove(info);
        }
        private bool CheckInterruption(IParticlesContainer newOrigin,out ParticlesMoving? interruptedProcess)
        {
            foreach(var mIn in MovingList)
            {
                if (mIn.Destination == newOrigin)
                {
                    interruptedProcess = mIn;
                    return true;
                }
            }
            interruptedProcess = null;
            return false;
        }
        private void InternalStartMove(ParticlesMoving info)
        {
            MovingList.AddLast(info);
            foreach(var par in info.Particles)
            {
                par.Owner.TurnOffPhysic();
            }
        }

        private void Awake()
        {
            Instance = this;
            enabled = false;
        }
        private void Update()
        {
            if (MovingList.Count_ > 0)
            {
                for(int i = 0; i < MovingList.Count_; i++)
                {
                    if (MovingList[i].Move())
                    {
                        MovingList.RemoveAtIndex(i);
                        i--;
                    }
                }
            }
        }
    }
}