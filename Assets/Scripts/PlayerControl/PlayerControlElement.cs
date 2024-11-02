

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
        protected virtual void AwakeAction() { }

        protected abstract void LockAction();
        protected abstract void UnlockAction();
    }
}