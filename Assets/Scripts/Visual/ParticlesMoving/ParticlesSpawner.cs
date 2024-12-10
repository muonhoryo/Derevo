


using UnityEngine;

namespace Derevo.Visual
{
    public sealed class ParticlesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ParticlePrefab;
        [SerializeField] private Transform ParticlesParent;

        private static ParticlesSpawner Instance;

        private void Awake()
        {
            Instance = this;
        }

        public static DiffusionParticle SpawnParticle(Vector2 position)
        {
            DiffusionParticle particle= Instantiate(Instance.ParticlePrefab, Instance.ParticlesParent).GetComponent<DiffusionParticle>();
            particle.transform.position = position;
            return particle;
        }
        public static DiffusionParticle[] SpawnParticles(Vector2 position,int count)
        {
            DiffusionParticle[] result=new DiffusionParticle[count];
            for(int i=0;i<count;i++)
            {
                result[i]=SpawnParticle(position);
            }
            return result;
        }
    }
}