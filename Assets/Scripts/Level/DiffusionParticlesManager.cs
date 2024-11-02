

using System;

namespace Derevo.Level
{
    public static class DiffusionParticlesManager
    {
        public struct RemainedParticlesChangedEventInfo
        {
            public int OldValue;
            public int NewValue;

            public RemainedParticlesChangedEventInfo(int oldValue, int newValue)
            {
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        public static event Action<RemainedParticlesChangedEventInfo> RemainedParticlesChangedEvent = delegate { };

        private static int MaxParticlesCount = 0;
        private static int RemainedParticlesCount = 0;
        public static int MaxParticlesCount_ => MaxParticlesCount;
        public static int RemainedParticlesCount_=>RemainedParticlesCount;

        public static void Initialize(int maxParticlesCount)
        {
            if (maxParticlesCount <= 0)
                return;

            MaxParticlesCount = maxParticlesCount;
            ChangeParticlesCount(MaxParticlesCount);
        }
        public static bool TryDecreaseParticlesCount(int decreaseValue)
        {
            if (RemainedParticlesCount < decreaseValue)
                return false;
            else
            {
                ChangeParticlesCount(RemainedParticlesCount - decreaseValue);
                return true;
            }
        }
        public static void IncreaseParticlesCount(int increaseValue)
        {
            ChangeParticlesCount(RemainedParticlesCount + increaseValue);
        }

        private static void ChangeParticlesCount(int newValue)
        {
            int oldCount = RemainedParticlesCount;
            RemainedParticlesCount = newValue;
            RemainedParticlesChangedEvent(new RemainedParticlesChangedEventInfo(oldCount, RemainedParticlesCount));
        }
    }
}