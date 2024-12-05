

using UnityEngine;
using UnityEngine.EventSystems;

namespace Derevo.UI
{
    public abstract class RadialSelector<T> : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}