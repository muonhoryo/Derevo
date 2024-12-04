

using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI
{
    public sealed class SpriteChangingButton_Press : MonoBehaviour
    {
        [SerializeField] private Image TargetSprite;
        [SerializeField] private UIButton TargetButton;
        [SerializeField] private Sprite NormalSprite;
        [SerializeField] private Sprite PressedSprite;

        private void Awake()
        {
            TargetButton.OnPointerDownEvent += OnPointerDown;
            TargetButton.OnPointerUpEvent += OnPointerUp;
            OnPointerUp();
        }
        private void OnPointerDown()
        {
            TargetSprite.sprite = PressedSprite;
        }
        private void OnPointerUp()
        {
            TargetSprite.sprite= NormalSprite;
        }
    }
}