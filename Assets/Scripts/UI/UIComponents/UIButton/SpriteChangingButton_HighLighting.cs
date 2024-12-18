

using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI
{
    public sealed class SpriteChangingButton_HighLighting : MonoBehaviour
    {
        [SerializeField] private Image TargetImage;
        [SerializeField] private UIButton TargetButton;
        [SerializeField] private Sprite NormalSprite;
        [SerializeField] private Sprite HighLightSprite;

        private void Awake()
        {
            TargetButton.OnPointerEnterEvent += OnPointerEnter;
            TargetButton.OnPointerExitEvent += OnPointerExit;
            TargetButton.DeactivationEvent += OnPointerExit;
            OnPointerExit();
        }
        private void OnDisable()
        {
            OnPointerExit();
        }
        private void OnPointerExit()
        {
            TargetImage.sprite = NormalSprite;
        }
        private void OnPointerEnter()
        {
            TargetImage.sprite= HighLightSprite;
        }
    }
}