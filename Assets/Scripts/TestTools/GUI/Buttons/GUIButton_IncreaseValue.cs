

using Derevo.PlayerControl;

namespace Derevo.GUI
{
    public sealed class GUIButton_IncreaseValue : GUIButton
    {
        protected override void OnClickAction()
        {
            UnfixedCellsControl.IncreaseValue();
        }
    }
}