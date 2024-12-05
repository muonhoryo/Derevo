

using Derevo.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI.Scripts
{
    public abstract class SldScript_AudioVolumeChanged : MonoBehaviour
    {
        [SerializeField] protected Slider Owner;

        public void ChangedVolumeBySlider()
        {
            InternalChangedVolumeBySlider();
        }

        protected abstract void InternalChangedVolumeBySlider();

        private void Awake()
        {
            SubscribeOnAudioManagerEvent();
        }
        private void OnDestroy()
        {
            UscribeFromAudioManagerEvent();
        }
        private void Start()
        {
            UpdateSliderValue();
        }
        protected abstract void SubscribeOnAudioManagerEvent();
        protected abstract void UscribeFromAudioManagerEvent();

        protected void ChangedVolumeByAudioManager(AudioManager.ChangeAudioVolumeEventInfo info)
        {
            if (!Mathf.Approximately(info.NewVolume, Owner.value))
                UpdateSliderValue();
        }
        protected abstract void UpdateSliderValue();
    }
}