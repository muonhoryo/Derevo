


using Derevo.Visual;
using UnityEngine;

namespace Derevo.UI
{
    public sealed class ButtonLock_ParticlesMoving : MonoBehaviour
    {
        [SerializeField] private UIButton Owner;

        private void Awake()
        {
            DiffParticlesMovingManager.ActivateParticlesMoving += ActivateMoving;
            DiffParticlesMovingManager.DeactivateParticlesMoving += DeactivateMoving;
        }
        private void OnDestroy()
        {
            DiffParticlesMovingManager.ActivateParticlesMoving -= ActivateMoving;
            DiffParticlesMovingManager.DeactivateParticlesMoving-= DeactivateMoving;
        }
        private void ActivateMoving()
        {
            Owner.Deactivate();
        }
        private void DeactivateMoving()
        {
            Owner.Activate();
        }
    }
}