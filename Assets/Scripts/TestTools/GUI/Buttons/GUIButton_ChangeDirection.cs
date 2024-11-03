
using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;

namespace Derevo.GUI
{
    public sealed class GUIButton_ChangeDirection : GUIButton
    {
        [SerializeField] private ValuableCell.DiffusionDirection Direction;

        protected override void OnClickAction()
        {
            UnfixedCellsControl.SetDiffusionDirection(Direction);
        }
    }
}