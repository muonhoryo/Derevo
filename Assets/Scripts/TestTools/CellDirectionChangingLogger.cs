


using UnityEngine;

namespace Derevo.Level
{
    public sealed class CellDirectionChangingLogger : MonoBehaviour
    {
        [SerializeField] private Vector2Int CellPos;

        private void Awake()
        {
            LevelManager.ChangeCellDiffDirectionEvent += ChangeCellDirection;
        }
        private void OnDestroy()
        {
            LevelManager.ChangeCellDiffDirectionEvent-= ChangeCellDirection;
        }
        private void ChangeCellDirection(LevelManager.ChangeCellDiffDIrectionEventInfo info)
        {
            if(info.Column==CellPos.x&&
                info.Row == CellPos.y)
            {
                Debug.Log("Change cell direction from " + info.OldDirection + " to " + info.NewDirection);
            }
        }
    }
}