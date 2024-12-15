

using System;

namespace Derevo.UI
{
    public static class CellsDisplayModeManager
    {
        public static event Action ActivateDigitalDisplayModeEvent = delegate { };
        public static event Action DeactivateDigitalDisplayModeEvent = delegate { };

        public static bool IsDigitalDisplayMode_ { get; private set; } = false;

        public static void ActivateDigitalDisplayMode()
        {
            if (!IsDigitalDisplayMode_)
            {
                IsDigitalDisplayMode_ = true;
                ActivateDigitalDisplayModeEvent();
            }
        }
        public static void DeactivateDigitalDisplayMode()
        {
            if (IsDigitalDisplayMode_)
            {
                IsDigitalDisplayMode_ = false;
                DeactivateDigitalDisplayModeEvent();
            }
        }
    }
}