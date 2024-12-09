
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
        public Vector2 CellsUploadPositionOffset;
    }
}