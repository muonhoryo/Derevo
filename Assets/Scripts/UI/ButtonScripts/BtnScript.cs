

using UnityEngine;

namespace Derevo.UI.Scripts
{
    public abstract class BtnScript : MonoBehaviour
    {
        [SerializeField] private UIButton Owner;

        private void Awake()
        {
            Owner.OnPointerDownEvent += OnPointerDown;
            AwakeAction();
        }
        protected virtual void AwakeAction() { }
        protected abstract void OnPointerDown();
    }
}