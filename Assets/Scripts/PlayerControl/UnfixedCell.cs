

using Derevo.Level;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Derevo.PlayerControl
{
    public sealed class UnfixedCell:PlayerControlElement,IPointerDownHandler
    {
        [SerializeField] public GameObject BlockCellVisual;

        private Vector2Int OwnerPos;
        public Vector2Int OwnerPos_
        {
            get => OwnerPos;
        }

        public void Initialize(Vector2Int OwnerPos)
        {
            if (!LevelManager.CheckCellPos(OwnerPos.x, OwnerPos.y))
                return;

            this.OwnerPos = OwnerPos;
        }
        private void OnDestroy()
        {
            LevelManager.ChangeCellDiffDirectionEvent -= ChangeDirectionAction;
        }
        protected override void AwakeAction()
        {
            LevelManager.ChangeCellDiffDirectionEvent += ChangeDirectionAction;
        }
        private void ChangeDirectionAction(LevelManager.ChangeCellDiffDIrectionEventInfo info)
        {
            if(info.Column==OwnerPos.x&&
                info.Row == OwnerPos.y)
            {
                if (info.NewDirection == 0 && info.ChangedCell.Value_ != 0)
                {
                    Destroy(this);
                }
            }
        }
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!PlayerControlLocker.IsLocked_)
            {
                UnfixedCellsManager.ReselectTarget(this);
            }
        }
        protected override void LockAction() { }
        protected override void UnlockAction() { }
    }
}