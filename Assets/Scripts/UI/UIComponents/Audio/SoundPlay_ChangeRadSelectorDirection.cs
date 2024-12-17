


using Derevo.Audio;
using UnityEngine;

namespace Derevo.UI
{
    public sealed class SoundPlay_ChangeRadSelectorDirection : MonoBehaviour
    {
        [SerializeField] private AudioClip Sound;
        [SerializeField] private RadialSelector Selector;

        private void Awake()
        {
            Selector.ChangeValueEvent += ChangeValue;
        }
        private void ChangeValue(float i,int j)
        {
            OneShotSoundPlayersManager.PlaySound(Sound);
        }
    }
}