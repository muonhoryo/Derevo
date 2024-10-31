

using UnityEngine;

namespace Derevo 
{
    public sealed class CoroutinesHandler : MonoBehaviour
    {
        public static CoroutinesHandler Instance_ { get; private set; }

        private void Awake()
        {
#if UNITY_EDITOR
            if (Instance_ != null)
                throw new System.Exception("Have more than one CoroutinesHandler");
            else
                Instance_ = this;
#else
            Instance_=this;
#endif
            Debug.Log(GlobalConstsHandler.Instance_.DiffusionProcessTime);
        }
    }
}