


using UnityEngine;

namespace Derevo.Level
{
    public sealed class UnfixedCellsManagerInitializator : MonoBehaviour
    {
        private void Awake()
        {
            DiffusionProcessing.DiffusionProcessing.StartDiffusionEvent += StartDiffusion;
        }
        private void OnDestroy()
        {
            DiffusionProcessing.DiffusionProcessing.StartDiffusionEvent -= StartDiffusion;
        }
        private void StartDiffusion()
        {
            UnfixedCellsManager.UnselectCurrentTarget();
        }
    }
}