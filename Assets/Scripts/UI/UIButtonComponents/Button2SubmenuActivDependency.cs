

using UnityEngine;

namespace Derevo.UI
{
    public sealed class Button2SubmenuActivDependency : MonoBehaviour
    {
        [SerializeField] private Submenu TargetSubmenu;
        [SerializeField] private UIButton OwnerButton;

        private void Awake()
        {
            TargetSubmenu.ShowingEvent += Showing;
            TargetSubmenu.HidingEvent += Hiding;
        }
        private void Showing()
        {
            OwnerButton.Deactivate();
        }
        private void Hiding()
        {
            OwnerButton.Activate();
        }
    }
}