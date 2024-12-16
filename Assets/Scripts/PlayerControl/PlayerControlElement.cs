

using UnityEngine;

namespace Derevo.PlayerControl 
{
    public abstract class PlayerControlElement : MonoBehaviour 
    {
        private void Awake()
        {
            PlayerControlLocker.LockControlEvent += LockAction;
            PlayerControlLocker.UnlockControlEvent += UnlockAction;
            AwakeAction();
        }
        private void OnDestroy()
        {
            PlayerControlLocker.LockControlEvent-= LockAction;
            PlayerControlLocker.UnlockControlEvent -= UnlockAction;
            OnDestroyAction();
        }
        protected virtual void AwakeAction() { }
        protected virtual void OnDestroyAction() { }

        protected abstract void LockAction();
        protected abstract void UnlockAction();
    }
}