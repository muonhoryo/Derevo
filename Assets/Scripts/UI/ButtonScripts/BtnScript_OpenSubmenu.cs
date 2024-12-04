

using UnityEngine;

namespace Derevo.UI.Scripts
{
    public sealed class BtnScript_OpenSubmenu : MonoBehaviour
    {
        [SerializeField] private UIButton Owner;
        [SerializeField] private Submenu Target;

        private void Awake()
        {
            Owner.OnPointerDownEvent += OnPointerDown;
        }
        private void OnPointerDown()
        {
            Target.Show();
        }
    }
}