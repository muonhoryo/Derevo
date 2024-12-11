

using UnityEngine;

namespace Derevo.Visual
{
    public sealed class DiffusionParticle : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D RGBody;
        [SerializeField] private Collider2D Collision;

        public void AddForce(Vector2 force)
        {
            RGBody.AddForce(force, ForceMode2D.Force);
        }
        /// <summary>
        /// Enable collision of particle
        /// </summary>
        public void TurnOnPhysic()
        {
            Collision.enabled = true;
        }
        /// <summary>
        /// Disable all physics of particle
        /// </summary>
        public void TurnOffPhysic()
        {
            Collision.enabled = false;
            TurnMovingOff();
        }
        /// <summary>
        /// Set particle as unmovable by physic
        /// </summary>
        public void TurnMovingOff()
        {
            RGBody.isKinematic = true;
            RGBody.velocity = Vector2.zero;
            RGBody.angularVelocity = 0;
        }
        /// <summary>
        /// Set particle as movable by physic
        /// </summary>
        public void TurnMovingOn()
        {
            RGBody.isKinematic = false;
            Collision.enabled = true;
        }
    }
}