

using UnityEngine;

namespace Derevo.UI.Scripts
{
    public sealed class BtnScript_ChangeUILayer : BtnScript
    {
        [SerializeField] private UILayer SelectLayer;
        protected override void OnPointerDown()
        {
            SelectLayer.SelectThis();
        }
    }
}