


using Derevo.Audio;
using Derevo.Level;
using UnityEngine;

namespace Derevo.UI
{
    public sealed class SoundPlay_SelectUnfixedCellTarget : MonoBehaviour
    {
        [SerializeField] private AudioClip Sound;

        private void Awake()
        {
            UnfixedCellsManager.ReselectTargetEvent += ReselectTarget;
        }
        private void OnDestroy()
        {
            UnfixedCellsManager.ReselectTargetEvent -= ReselectTarget;
        }
        private void ReselectTarget(UnfixedCellsManager.ReselectTargetEventInfo i)
        {
            OneShotSoundPlayersManager.PlaySound(Sound);
        }
    }
}