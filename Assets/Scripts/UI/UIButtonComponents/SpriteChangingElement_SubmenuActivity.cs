

using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI.Scripts
{
    public sealed class SpriteChangingElement_SubmenuActivity : MonoBehaviour
    {
        [SerializeField] private Image TargetSprite;
        [SerializeField] private Submenu DependedSubmenu;
        [SerializeField] private Sprite NormalSprite;
        [SerializeField] private Sprite ActiveSubmenuSprite;

        private void Awake()
        {
            DependedSubmenu.ShowingEvent += Activation;
            DependedSubmenu.HidingEvent += Deactivation;
            Deactivation();
        }
        private void Activation()
        {
            TargetSprite.sprite = ActiveSubmenuSprite;
        }
        private void Deactivation()
        {
            TargetSprite.sprite = NormalSprite;
        }
    }
}