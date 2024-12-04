

using UnityEngine;

namespace Derevo.UI.Scripts
{
    public sealed class BtnScript_ChangeUILayer : MonoBehaviour
    {
        [SerializeField] private UIButton Owner;
        [SerializeField] private UILayer SelectLayer;

        private void Awake()
        {
            Owner.OnPointerDownEvent += OnPointerDown;
        }
        private void OnPointerDown()
        {
            SelectLayer.SelectThis();
        }
    }
}