

using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI
{
    public sealed class SpriteChangingButton_Select : MonoBehaviour
    {
        [SerializeField] private Image TargetSprite;
        [SerializeField] private UIButton TargetButton;
        [SerializeField] private Sprite SelectSprite;

        private void Awake()
        {
            TargetButton.OnPointerDownEvent += OnPointerDown;
        }
        private void OnPointerDown() 
        {
            TargetSprite.sprite = SelectSprite; 
        }
    }
}