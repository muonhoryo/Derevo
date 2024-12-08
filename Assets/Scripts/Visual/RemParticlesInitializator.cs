

using Derevo.Level;
using UnityEngine;
using System.Linq;

namespace Derevo.Visual
{
    public sealed class RemParticlesInitializator : MonoBehaviour 
    {
        [SerializeField] private GameObject ParticlePrefab;
        [SerializeField] private IParticlesContainer RemParticlesIndicator;

        private void Awake()
        {
            LevelManager.InitializeMapEvent += LevelInitialization;
        }
        private void LevelInitialization()
        {
            LevelManager.InitializeMapEvent -= LevelInitialization;
            DiffusionParticle[] particles = ParticlesSpawner.SpawnParticles(RemParticlesIndicator.UploadPosition_,
                DiffusionParticlesManager.RemainedParticlesCount_);
            DiffParticlesMovingManager.MoveDirectly(particles, RemParticlesIndicator, Enumerable.Repeat(float.MaxValue, particles.Length).ToArray());
        }
    }
}