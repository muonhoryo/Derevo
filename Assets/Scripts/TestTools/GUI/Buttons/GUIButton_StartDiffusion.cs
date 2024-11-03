

using Derevo.PlayerControl;

namespace Derevo.GUI
{
    public sealed class GUIButton_StartDiffusion : GUIButton
    {
        protected override void OnClickAction()
        {
            UnfixedCellsControl.StartDiffusion();
        }
    }
}