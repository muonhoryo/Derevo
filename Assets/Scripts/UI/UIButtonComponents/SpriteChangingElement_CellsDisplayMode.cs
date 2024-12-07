

using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI.Scripts
{
    public sealed class SpriteChangingElement_CellsDisplayMode : MonoBehaviour
    {
        [SerializeField] private Image TargetSprite;
        [SerializeField] private Sprite NormalSprite;
        [SerializeField] private Sprite DisplayModeSprite;

        private void Awake()
        {
            CellsDisplayModeManager.ActivateDigitalDisplayModeEvent += ActivateMode;
            CellsDisplayModeManager.DeactivateDigitalDisplayModeEvent+= DeactivateMode;
            if (CellsDisplayModeManager.IsDigitalDisplayMode_)
                ActivateMode();
            else
                DeactivateMode();
        }
        private void OnDestroy()
        {
            CellsDisplayModeManager.ActivateDigitalDisplayModeEvent -= ActivateMode;
            CellsDisplayModeManager.DeactivateDigitalDisplayModeEvent -= DeactivateMode;
        }
        private void ActivateMode()
        {
            TargetSprite.sprite = DisplayModeSprite;
        }
        private void DeactivateMode()
        {
            TargetSprite.sprite = NormalSprite;
        }
    }
}