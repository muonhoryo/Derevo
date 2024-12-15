


using Derevo.PlayerControl;
using Derevo.UI.Scripts;

namespace Derevo.UI
{
    public sealed class BtnScript_StartDiffusion : BtnScript
    {
        protected override void OnPointerDown()
        {
            UnfixedCellsControl.StartDiffusion();
        }
    }
}