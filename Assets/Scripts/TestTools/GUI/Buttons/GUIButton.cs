

using UnityEngine;
using UnityEngine.EventSystems;

namespace Derevo.GUI
{
    public abstract class GUIButton : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            OnClickAction();
        }
        protected abstract void OnClickAction();
    }
}