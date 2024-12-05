

using UnityEngine;
using UnityEngine.Audio;
using MuonhoryoLibrary.Unity;

namespace Derevo.Audio
{
    public sealed class AudioMixerHandler : MonoBehaviour
    {
        [SerializeField] private string MusicAttenuationName;
        [SerializeField] private string SoundsAttenuationName;

        [SerializeField] private AudioMixer Target;

        private void Awake()
        {
            AudioManager.ChangeMusicVolumeEvent += MusicVolumeChanged;
            AudioManager.ChangeSoundsVolumeEvent += SoundsVolumeChanged;
            DontDestroyOnLoad(gameObject);
        }
        private void MusicVolumeChanged(AudioManager.ChangeAudioVolumeEventInfo info)
        {
            ChangeAudioVolume(info.NewVolume, MusicAttenuationName);
        }
        private void SoundsVolumeChanged(AudioManager.ChangeAudioVolumeEventInfo info)
        {
            ChangeAudioVolume(info.NewVolume, SoundsAttenuationName);
        }
        private void ChangeAudioVolume(float volume,string groupName)
        {
            Target.SetFloat(groupName, volume.VolumeLevelToDB());
        }
    }
}