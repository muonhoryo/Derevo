

using Derevo.UI;
using UnityEngine;

namespace Derevo.PlayerControl
{
    public sealed class CellsDisplayModeControl : MonoBehaviour
    {
        [SerializeField] private string ChangeDisplayModeInputName;

        private void Awake()
        {
            if (Input.GetButton(ChangeDisplayModeInputName))
                CellsDisplayModeManager.ActivateDigitalDisplayMode();
            else
                CellsDisplayModeManager.DeactivateDigitalDisplayMode();
        }
        private void OnDestroy()
        {
            CellsDisplayModeManager.DeactivateDigitalDisplayMode();
        }
        private void Update()
        {
            if (Input.GetButtonDown(ChangeDisplayModeInputName))
                CellsDisplayModeManager.ActivateDigitalDisplayMode();
            else if (Input.GetButtonUp(ChangeDisplayModeInputName))
                CellsDisplayModeManager.DeactivateDigitalDisplayMode();
        }
    }
}