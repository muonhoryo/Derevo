

using Derevo.Level;
using UnityEngine;

namespace Derevo.PlayerControl 
{
    public sealed class UnfixedCellsControl : MonoBehaviour
    {
        public void IncreaseValue()
        {
            UnfixedCellsManager.TrySetValue(UnfixedCellsManager.GetTargetValue() + 1);
        }
        public void DecreaseValue()
        {
            UnfixedCellsManager.TrySetValue(UnfixedCellsManager.GetTargetValue() - 1);
        }
        public void SetDiffusionDirection(ValuableCell.DiffusionDirection direction)
        {
            UnfixedCellsManager.TrySetDiffusionDirection(direction);
        }
        public void StartDiffusion()
        {
            DiffusionProcessing.DiffusionProcessing.StartDiffusion();
        }
    }
}