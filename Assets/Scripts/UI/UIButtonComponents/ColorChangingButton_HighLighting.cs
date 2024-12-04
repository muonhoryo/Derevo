

using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI
{
    public sealed class ColorChangingButton_HighLighting : MonoBehaviour
    {
        [SerializeField] private Graphic TargetGraphic;
        [SerializeField] private UIButton TargetButton;
        [SerializeField] private Color NormalColor;
        [SerializeField] private Color HighLightColor;

        private void Awake()
        {
            TargetButton.OnPointerEnterEvent += OnPointerEnter;
            TargetButton.OnPointerExitEvent += OnPointerExit;
            TargetButton.DeactivationEvent += OnPointerExit;
            OnPointerExit();
        }
        private void OnPointerExit()
        {
            TargetGraphic.color = NormalColor;
        }
        private void OnPointerEnter()
        {
            TargetGraphic.color = HighLightColor;
        }
    }
}