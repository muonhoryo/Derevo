

using System;
using Derevo.PlayerControl;
using UnityEngine;

namespace Derevo.Level
{
    public static class UnfixedCellsManager
    {
        public struct ReselectTargetEventInfo
        {
            public UnfixedCell OldTarget;
            public UnfixedCell NewTarget;

            public ReselectTargetEventInfo(UnfixedCell oldTarget, UnfixedCell newTarget)
            {
                OldTarget = oldTarget;
                NewTarget = newTarget;
            }
        }

        public static event Action<ReselectTargetEventInfo> ReselectTargetEvent = delegate { };
        public static event Action<UnfixedCell> UnselectTargetEvent = delegate { };

        private static UnfixedCell Target;
        public static UnfixedCell Target_
        {
            get => Target;
        }

        public static void ReselectTarget(UnfixedCell cell)
        {
            if (cell == null||
                cell==Target)
                return;
            ValuableCell valCell = GetCellFromLevelManager(cell);
            if (valCell==null|| !valCell.IsUnfixed_)
                return;

            var info = new ReselectTargetEventInfo(Target, cell);
            if (valCell.Value_== 0) //Mark cell as unfixed
            {
                if (DiffusionParticlesManager.TryDecreaseParticlesCount(1))
                {
                    LevelManager.TrySetCellDefaultDiffDirection(cell.OwnerPos_.x, cell.OwnerPos_.y);
                    LevelManager.TrySetCellValue(1, cell.OwnerPos_.x, cell.OwnerPos_.y);
                }
            }
            Target = cell;
            ReselectTargetEvent(info);
        }
        public static void UnselectCurrentTarget()
        {
            if( Target == null ) 
                return;

            UnfixedCell oldTarget = Target;
            Target = null;
            UnselectTargetEvent(oldTarget);
        }
        public static bool TrySetDiffusionDirection(ValuableCell.DiffusionDirection direction)
        {
            if (Target == null||
                direction>=ValuableCell.DiffusionDirection.CannotHaveDirections)
                return false;
            return LevelManager.TrySetCellDiffusionDirection(direction, Target.OwnerPos_.x, Target.OwnerPos_.y);
        }
        public static bool TrySetValue(int value)
        {
            if (Target == null)
                return false;
            if (value < 0)
                return false;

            ValuableCell cell = GetCellFromLevelManager(Target);
            int diff = value - cell.Value_;
            if (diff == 0)
                return false;
            else if (diff < 0) //Value of cell is decresing
            {
                DiffusionParticlesManager.IncreaseParticlesCount(-diff);
                bool result= LevelManager.TrySetCellValue(value, Target.OwnerPos_.x, Target.OwnerPos_.y);
                if (cell.Value_ == 0)
                {
                    LevelManager.TrySetCellDiffusionDirection(0, Target.OwnerPos_.x, Target.OwnerPos_.y);
                    UnselectCurrentTarget();
                }
                return result;
            }
            else //Value of cell is Increasing
            {
                if (DiffusionParticlesManager.TryDecreaseParticlesCount(diff))
                {
                    bool result = LevelManager.TrySetCellValue(value, Target.OwnerPos_.x, Target.OwnerPos_.y);
                    return result;
                }
                else
                    return false; 
            }
        }
        public static int GetTargetValue()
        {
            if (Target == null)
                return 0;
            return GetCellFromLevelManager(Target).Value_;
        }

        private static ValuableCell GetCellFromLevelManager(UnfixedCell cell)
        {
            return LevelManager.GetCell(cell.OwnerPos_.x, cell.OwnerPos_.y) as ValuableCell;
        }
    }
}