


using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Derevo.UI
{
    public sealed class AnchorSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<float> ChangeValueEvent = delegate { };
        public event Action ResetSliderEvent = delegate { };

        [SerializeField] private bool IsVertical= true;
        [SerializeField] private RectTransform HandlerRect;
        [SerializeField] private float MaxValue;

        private float HalfSize;
        private float HalfRectSize;
        private RectTransform Rect;
        public float Value_ { get; private set; }
        public bool IsHolding_ { get; private set; } = false;

        private void Awake()
        {
            Rect= transform as RectTransform;
            if (IsVertical)
            {
                HalfRectSize = Rect.rect.height/2;
                HalfSize = HalfRectSize * transform.lossyScale.y;
            }
            else
            {
                HalfRectSize = Rect.rect.width/2;
                HalfSize = HalfRectSize * transform.lossyScale.x;
            }
        }
        private void Update()
        {
            if(IsHolding_)
                OnHold(Input.mousePosition); 
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnHold(eventData.position);
            IsHolding_ = true;
        }
        private void OnHold(Vector2 mouseScreenPos)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            float diff = IsVertical ? mousePos.y - transform.position.y : mousePos.x - transform.position.x;
            diff = Mathf.Clamp(diff, -HalfSize, HalfSize);
            float magnitude = diff / HalfSize;
            float newValue = magnitude * MaxValue;
            if (newValue != Value_)
            {
                ChangeValue(newValue, magnitude * HalfRectSize);
            }
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            ChangeValue(0, 0);
            IsHolding_ = false;
        }
        private void ChangeValue(float value,float handlerPos)
        {
            Value_ = value;
            HandlerRect.localPosition = new Vector3(
                IsVertical ? HandlerRect.localPosition.x : handlerPos,
                IsVertical ? handlerPos : HandlerRect.localPosition.y,
                HandlerRect.localPosition.z);
            ChangeValueEvent(Value_);
        }
    }
}