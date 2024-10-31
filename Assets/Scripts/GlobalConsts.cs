
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
    }
}