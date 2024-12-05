

using Derevo.Audio;
using Derevo.UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI
{
    public sealed class SldScript_ChangeSoundsVolume : SldScript_AudioVolumeChanged
    {
        protected override void InternalChangedVolumeBySlider()
        {
            AudioManager.SoundsVolume_ = Owner.value;
        }

        protected override void SubscribeOnAudioManagerEvent()
        {
            AudioManager.ChangeSoundsVolumeEvent += ChangedVolumeByAudioManager;
        }

        protected override void UpdateSliderValue()
        {
            Owner.value = AudioManager.SoundsVolume_;
        }

        protected override void UscribeFromAudioManagerEvent()
        {
            AudioManager.ChangeSoundsVolumeEvent -= ChangedVolumeByAudioManager;
        }
    }
}