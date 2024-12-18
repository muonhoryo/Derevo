


using UnityEngine;

namespace Derevo.Visual
{
    public sealed class VisualPhysicHandler : MonoBehaviour
    {
        public static VisualPhysicHandler Instance_ { get; private set; }

        [SerializeField] public int ParticlesOverlapMask;

        private void Awake()
        {
            Instance_ = this;
        }
    }
}