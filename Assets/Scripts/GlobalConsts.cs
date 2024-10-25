
using UnityEngine;

namespace Derevo 
{
    [CreateAssetMenu]
    public sealed class GlobalConsts : ScriptableObject
    {
        [Range(0,100)]
        public float DiffusionProcessTime;
    }
}