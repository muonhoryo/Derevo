

using UnityEngine;

namespace Derevo 
{
    public sealed class CoroutinesHandler : MonoBehaviour
    {
        public static CoroutinesHandler Instance_ { get; private set; }

        private void Awake()
        {
            if (Instance_ != null)
                Destroy(this);

            Instance_ = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}