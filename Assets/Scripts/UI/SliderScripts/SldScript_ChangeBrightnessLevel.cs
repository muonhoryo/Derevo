

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Derevo.UI.Scripts
{
    public sealed class SldScript_ChangeBrightnessLevel : MonoBehaviour
    {
        [SerializeField] private Slider Owner;
        [SerializeField] private PostProcessProfile TargetProfile;

        private AutoExposure BrightnessEffect;

        private void Awake()
        {
            TargetProfile.TryGetSettings(out BrightnessEffect);
        }
        private void Start()
        {
            Owner.value = BrightnessEffect.keyValue;
        }

        public void ChangeBrightness()
        {
            BrightnessEffect.keyValue.value = Owner.value;
        }

    }
}