

using System.Collections;
using System.Collections.Generic;
using Derevo.Audio;
using UnityEngine;


namespace Derevo.UI
{
    public sealed class SoundPlay_OnButtonDown : MonoBehaviour
    {
        [SerializeField] private UIButton Button;
        [SerializeField] private AudioClip PlayedSound;

        private void Awake()
        {
            Button.OnPointerDownEvent += OnPointerDown;
        }
        private void OnPointerDown()
        {
            OneShotSoundPlayersManager.PlaySound(PlayedSound);
        }
    }
}
