

using System;
using System.Buffers;
using Derevo.PlayerControl;

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
            if (cell == null )
                return;
            ValuableCell valCell = GetCellFromLevelManager();
            if (valCell==null||
                (valCell.Value_ != 0 && valCell.DiffusionDirection_ == 0))
                return;

            var info = new ReselectTargetEventInfo(Target, cell);
            Target = cell;
            if (valCell.Value_== 0) //Mark cell as unfixed
            {
                valCell.SetDefaultDirection(Target.OwnerPos_);
                valCell.TrySetValue(1);
            }

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
            return LevelManager.TrySetCellDiffusionDirection(direction, Target.OwnerPos_.x, Target.OwnerPos_.y);
        }
        public static bool TrySetValue(int value)
        {
            if (value < 0)
                return false;

            ValuableCell cell = GetCellFromLevelManager();
            int diff = value - cell.Value_;
            if (diff == 0)
                return false;
            else if (diff < 0) //Value of cell is decresing
            {
                DiffusionParticlesManager.IncreaseParticlesCount(-diff);
                bool result= LevelManager.TrySetCellValue(value, Target.OwnerPos_.x, Target.OwnerPos_.y);
                if (cell.Value_ == 0)
                {
                    cell.TrySetDiffusionDirection(0, Target.OwnerPos_.x, Target.OwnerPos_.y);
                    UnselectCurrentTarget();
                }
                return result;
            }
            else //Value of cell is Increasing
            {
                if (DiffusionParticlesManager.TryDecreaseParticlesCount(diff))
                {
                    return LevelManager.TrySetCellValue(value, Target.OwnerPos_.x, Target.OwnerPos_.y);
                }
                else
                    return false;
            }
        }
        public static int GetTargetValue() =>
            GetCellFromLevelManager().Value_;

        private static ValuableCell GetCellFromLevelManager()
        {
            return LevelManager.GetCell(Target.OwnerPos_.x, Target.OwnerPos_.y) as ValuableCell;
        }
    }
}