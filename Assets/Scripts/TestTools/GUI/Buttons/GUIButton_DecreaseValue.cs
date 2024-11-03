

using Derevo.PlayerControl;

namespace Derevo.GUI
{
    public sealed class GUIButton_DecreaseValue : GUIButton
    {
        protected override void OnClickAction()
        {
            UnfixedCellsControl.DecreaseValue();
        }
    }
}