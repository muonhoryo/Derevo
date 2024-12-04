

using UnityEngine;

namespace Derevo.UI.Scripts
{
    public sealed class BtnScript_OpenSubmenu : BtnScript
    {
        [SerializeField] private Submenu Target;
         
        protected override void OnPointerDown()
        {
            Target.Show();
        }
    }
}