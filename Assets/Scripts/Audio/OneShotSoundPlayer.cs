

using System.Collections;
using UnityEngine;

namespace Derevo.Audio
{
    public sealed class OneShotSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource AudioSource;

        private IEnumerator DestroyDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(obj);
        }
        public void PlaySound(AudioClip playedClip)
        {
            AudioSource.clip = playedClip;
            AudioSource.Play();
            StartCoroutine(DestroyDelay(AudioSource.gameObject, playedClip.length));
        }
    }
}