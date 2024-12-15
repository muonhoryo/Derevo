


using UnityEngine;

namespace Derevo.UI
{
    public sealed class ElementActivDepend_CellDisplayMode : MonoBehaviour
    {
        [SerializeField] private GameObject Target;

        private void Awake()
        {
            CellsDisplayModeManager.ActivateDigitalDisplayModeEvent += Activate;
            CellsDisplayModeManager.DeactivateDigitalDisplayModeEvent += Deactivate;
            Deactivate();
        }
        private void OnDestroy()
        {
            CellsDisplayModeManager.ActivateDigitalDisplayModeEvent -= Activate;
            CellsDisplayModeManager.DeactivateDigitalDisplayModeEvent -= Deactivate;
        }
        private void Activate()
        {
            Target.SetActive(true);
        }
        private void Deactivate()
        {
            Target.SetActive(false);
        }
    }
}