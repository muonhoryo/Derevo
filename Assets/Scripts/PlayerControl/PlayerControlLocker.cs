

using System;

namespace Derevo.PlayerControl
{
    public static class PlayerControlLocker
    {
        public static event Action LockControlEvent = delegate { };
        public static event Action UnlockControlEvent = delegate { };

        public static bool IsLocked_ { get; private set; } = false;

        public static void Lock()
        {
            if(!IsLocked_)
            {
                IsLocked_ = true;
                LockControlEvent();
            }
        }
        public static void Unlock()
        {
            if (IsLocked_)
            {
                IsLocked_ = false;
                UnlockControlEvent();
            }
        }
    }
}