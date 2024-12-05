

using Derevo.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI.Scripts
{
    public sealed class SldScript_ChangeMusicVolume : SldScript_AudioVolumeChanged
    {
        protected override void InternalChangedVolumeBySlider()
        {
            AudioManager.MusicVolume_ = Owner.value;
        }

        protected override void SubscribeOnAudioManagerEvent()
        {
            AudioManager.ChangeMusicVolumeEvent += ChangedVolumeByAudioManager;
        }

        protected override void UpdateSliderValue()
        {
            Owner.value = AudioManager.MusicVolume_;
        }

        protected override void UscribeFromAudioManagerEvent()
        {
            AudioManager.ChangeMusicVolumeEvent -= ChangedVolumeByAudioManager;
        }
    }
}