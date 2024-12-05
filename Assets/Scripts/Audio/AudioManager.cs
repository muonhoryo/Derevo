

using System;

namespace Derevo.Audio
{
    public static class AudioManager
    {
        public struct ChangeAudioVolumeEventInfo
        {
            public ChangeAudioVolumeEventInfo(float NewVolume,float OldVolume)
            {
                this.NewVolume = NewVolume;
                this.OldVolume= OldVolume;
            }
            public float NewVolume;
            public float OldVolume;
        }
        public static event Action<ChangeAudioVolumeEventInfo> ChangeMusicVolumeEvent = delegate { };
        public static event Action<ChangeAudioVolumeEventInfo> ChangeSoundsVolumeEvent = delegate { };

        private static float MusicVolume=1;
        private static float SoundsVolume = 1;

        public static float MusicVolume_
        {
            get => MusicVolume;
            set
            {
                if (value > 1)
                    SetMusicVolume(1);
                else if (value < 0)
                    SetMusicVolume(0);
                else
                    SetMusicVolume(value);
            }
        }
        public static float SoundsVolume_
        {
            get => SoundsVolume;
            set
            {
                if (value > 1)
                    SetSoundsVolume(1);
                else if (value < 0)
                    SetSoundsVolume(0);
                else
                    SetSoundsVolume(value);
            }
        }

        private static void SetMusicVolume(float volume)
        {
            float oldVol = MusicVolume;
            MusicVolume = volume;
            ChangeMusicVolumeEvent(new ChangeAudioVolumeEventInfo(volume, oldVol));
        }
        private static void SetSoundsVolume(float volume)
        {
            float oldVol = SoundsVolume;
            SoundsVolume = volume;
            ChangeSoundsVolumeEvent(new ChangeAudioVolumeEventInfo(volume, oldVol));
        }
    }
}