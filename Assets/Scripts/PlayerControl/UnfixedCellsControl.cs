

using Derevo.Level;
using UnityEngine;

namespace Derevo.PlayerControl 
{
    public static class UnfixedCellsControl
    {
        public static void IncreaseValue()
        {
            UnfixedCellsManager.TrySetValue(UnfixedCellsManager.GetTargetValue() + 1);
        }
        public static void DecreaseValue()
        {
            UnfixedCellsManager.TrySetValue(UnfixedCellsManager.GetTargetValue() - 1);
        }
        public static void SetDiffusionDirection(ValuableCell.DiffusionDirection direction)
        {
            UnfixedCellsManager.TrySetDiffusionDirection(direction);
        }
        public static void StartDiffusion()
        {
            DiffusionProcessing.DiffusionProcessing.StartDiffusion();
        }
    }
}