

using Derevo.PlayerControl;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Derevo.GUI
{
    public abstract class GUIButton : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!PlayerControlLocker.IsLocked_)
            {
                OnClickAction();
            }
        }
        protected abstract void OnClickAction();
    }
}