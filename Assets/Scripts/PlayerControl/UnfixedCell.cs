

using Derevo.Level;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Derevo.PlayerControl
{
    public sealed class UnfixedCell:PlayerControlElement,IPointerDownHandler
    {
        private Vector2Int OwnerPos;
        public Vector2Int OwnerPos_
        {
            get => OwnerPos;
            set
            {
                if (!LevelManager.CheckCellPos(value.x, value.y))
                    return;

                OwnerPos = value;
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if(!PlayerControlLocker.IsLocked_)
                UnfixedCellsManager.ReselectTarget(this);
        }
        protected override void LockAction() { }
        protected override void UnlockAction() { }
    }
}