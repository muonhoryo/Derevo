

using UnityEngine;

namespace Derevo.UI.Scripts
{
    public sealed class BtnScript_DeactivateAllCurrButtons : BtnScript
    {
        protected override void OnPointerDown()
        {
            var btns= FindObjectsOfType<UIButton>();
            foreach (var btn in btns)
                btn.Deactivate();
        }
    }
}