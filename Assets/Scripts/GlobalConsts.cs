
using System;
using UnityEngine;

namespace Derevo 
{
    [CreateAssetMenu]
    [Serializable]
    public sealed class GlobalConsts : ScriptableObject
    {
        [Range(0,100)]
        public float DiffusionProcessTime;

        [Range(0, 100)]
        public float LevelTransitionDelay;
        [Range(0,10)]
        public float ParticlesFermentationThreshold;
        [Range(0,100)]
        public float ParticlesFixingRadius;
        [Range(1,100)]
        public int RemParticlesVisualIndicator_FreeRowsCount;
        [Range(0.001f,10)]
        public float RemParticlesVisualIndicator_HighestRowCalculationDelay;
        [Range(0,100)]
        public float RemParticlesVisualIndicator_MinForce;
        [Range(0,100)]
        public float RemParticlesVisualIndicator_MaxForce;
        [Range(0,10)]
        public float ParticlesDiffusionSpeedMin;
        [Range(0,10)]
        public float ParticlesDiffusionSpeedDispersion;
        [Range(0,10)]
        public float ParticlesAdditionalPathLength;
        public Vector2 CellsUploadPositionOffset;
    }
}