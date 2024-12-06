

using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI
{
    public sealed class SpriteChangingButton_Activation : MonoBehaviour
    {
        [SerializeField] private Image TargetSprite;
        [SerializeField] private UIButton TargetButton;
        [SerializeField] private Sprite NormalSprite;
        [SerializeField] private Sprite InactiveSprite;

        private void Awake()
        {
            TargetButton.ActivationEvent += Activation;
            TargetButton.DeactivationEvent+= Deactivation;
            Activation();
        }
        private void Deactivation()
        {
            TargetSprite.sprite = InactiveSprite;
        }
        private void Activation()
        {
            TargetSprite.sprite = NormalSprite;
        }
    }
}