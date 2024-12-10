

using Derevo.Level;
using UnityEngine;
using System.Linq;

namespace Derevo.Visual
{
    public sealed class RemParticlesInitializator : MonoBehaviour 
    {
        [SerializeField] private MonoBehaviour RemParticlesIndicator;

        private void Awake()
        {
            LevelManager.InitializeMapEvent += LevelInitialization;
        }
        private void LevelInitialization()
        {
            LevelManager.InitializeMapEvent -= LevelInitialization; 
            IParticlesContainer indicator = RemParticlesIndicator as IParticlesContainer;
            DiffusionParticle[] particles = ParticlesSpawner.SpawnParticles(indicator.UploadPosition_,
                DiffusionParticlesManager.RemainedParticlesCount_);
            DiffParticlesMovingManager.MoveDirectly(particles, indicator, Enumerable.Repeat(float.MaxValue, particles.Length).ToArray());
        }
    }
}