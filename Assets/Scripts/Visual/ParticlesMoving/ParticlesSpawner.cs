


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
            particle.transform.position = new Vector3(position.x, position.y, 0);
            return particle;
        }
        public static DiffusionParticle[] SpawnParticles(Vector2 position,int count)
        {
            DiffusionParticle[] result=new DiffusionParticle[count];
            float maxDisp = GlobalConstsHandler.Instance_.ParticlesSpawner_PositionDispersion;
            float minDisp = -maxDisp;
            for (int i=0;i<count;i++)
            {
                Vector2 offset = new Vector2(Random.Range(minDisp, maxDisp), Random.Range(minDisp, maxDisp));
                result[i]=SpawnParticle(position+offset);
            }
            return result;
        }
    }
}