


using UnityEngine;

namespace Derevo.Audio
{
    public sealed class OneShotSoundPlayersManager : MonoBehaviour
    {
        private static OneShotSoundPlayersManager Instance;

        [SerializeField] private GameObject SoundPlayerPrefab;
        [SerializeField] private Transform SoundsParent;

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void PlaySound(AudioClip clip)
        {
            Instance.PlaySound_NoSt(clip);
        }

        private void PlaySound_NoSt(AudioClip clip)
        {
            OneShotSoundPlayer player = Instantiate(SoundPlayerPrefab, SoundsParent).GetComponent<OneShotSoundPlayer>();
            player.PlaySound(clip);
        }
    }
}