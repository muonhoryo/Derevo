


using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;

namespace Derevo.UI
{
    public sealed class RadialSelector_ChangeDiffusionDirection : MonoBehaviour
    {
        [SerializeField] private RadialSelector OwnerSelector;

        private void Awake()
        {
            UnfixedCellsManager.ReselectTargetEvent += ReselectTarget;
            OwnerSelector.ChangeValueEvent += ChangeValue;
        }
        private void OnDestroy()
        {
            UnfixedCellsManager.ReselectTargetEvent -= ReselectTarget;
        }
        private void ReselectTarget(UnfixedCellsManager.ReselectTargetEventInfo info)
        {
            ValuableCell owner = LevelManager.GetCell(info.NewTarget.OwnerPos_.x, info.NewTarget.OwnerPos_.y) as ValuableCell;
            if (AbleToHaveDiffDirection(owner.DiffusionDirection_))
            {
                OwnerSelector.ChangeValueByIndex(GetDigitByDirection(owner.DiffusionDirection_));
            }
        }
        private void ChangeValue(float value,int directionIndex)
        {
            if (UnfixedCellsManager.Target_ != null)
            {
                UnfixedCellsControl.SetDiffusionDirection(GetDirectionByDigit(directionIndex));
            }
        }

        private int GetDigitByDirection(ValuableCell.DiffusionDirection direction)
        {
            int parDirection = (int)direction;
            int i = 1;
            int digit = 0;
            while (i <= ValuableCell.MaxDiffusionDirectionValue)
            {
                if((parDirection&i)!=0)
                {
                    return digit;
                }
                else
                {
                    i = i << 1;
                    digit++;
                }
            }
            throw new System.Exception("Error with digit of direction calculation.");
        }
        private ValuableCell.DiffusionDirection GetDirectionByDigit(int digit)
        {
            return (ValuableCell.DiffusionDirection)(1 << digit);
        }
        private bool AbleToHaveDiffDirection(ValuableCell.DiffusionDirection direction)
        {
            return (direction & ValuableCell.DiffusionDirection.CannotHaveDirections) == 0;
        }
    }
}