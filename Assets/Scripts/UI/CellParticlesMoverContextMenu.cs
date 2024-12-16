


using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;

namespace Derevo.UI
{
    public sealed class CellParticlesMoverContextMenu : MonoBehaviour
    {
        [SerializeField] private Vector3 Offset;

        private void Awake()
        {
            UnfixedCellsManager.ReselectTargetEvent += ReselectTarget;
            UnfixedCellsManager.UnselectTargetEvent+= UnselectTarget;
        }
        private void Start()
        {
            gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            UnfixedCellsManager.ReselectTargetEvent-= ReselectTarget;
            UnfixedCellsManager.UnselectTargetEvent -= UnselectTarget;
        }
        private void ReselectTarget(UnfixedCellsManager.ReselectTargetEventInfo info)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            transform.position=info.NewTarget.transform.position+Offset;
        }
        private void UnselectTarget(UnfixedCell i)
        {
            gameObject.SetActive(false);
        }
    }
}