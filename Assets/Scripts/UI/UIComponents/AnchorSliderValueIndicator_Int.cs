


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Derevo.UI
{
    public sealed class AnchorSliderValueIndicator_Int : MonoBehaviour
    {
        [SerializeField] private AnchorSlider Slider;
        [SerializeField] private Text Text;

        private void Awake()
        {
            Slider.ChangeValueEvent += ChangeValue;
            Slider.ResetSliderEvent+= ResetSlider;
        }
        private void Start()
        {
            ChangeValue(Slider.Value_);
        }
        private void ChangeValue(float value)
        {
            Text.text = ((int)value).ToString();
        }
        private void ResetSlider()
        {
            ChangeValue(Slider.Value_);
        }
    }
}