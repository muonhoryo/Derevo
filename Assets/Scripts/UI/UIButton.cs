

using System;
using Derevo.PlayerControl;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Derevo.UI
{
    public class UIButton : PlayerControlElement, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action OnPointerDownEvent = delegate { };
        public event Action OnPointerUpEvent = delegate { };
        public event Action OnPointerEnterEvent = delegate { };
        public event Action OnPointerExitEvent = delegate { };
        public event Action ActivationEvent = delegate { };
        public event Action DeactivationEvent = delegate { };

        public bool IsActive_ { get; private set; } = true;
        public bool IsInsidePointer_ { get; private set; } = false;
        public bool IsPressed_ { get; private set; } = false;

        public void Activate()
        {
            if(!IsActive_)
            {
                IsActive_ = true;
                ActivationEvent();
            }
        }
        public void Deactivate()
        {
            if (IsActive_)
            {
                IsActive_ = false;
                DeactivationEvent();
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (IsActive_)
            {
                IsPressed_ = true;
                OnPointerDownEvent();
            }
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (IsActive_ && IsInsidePointer_)
            {
                IsPressed_ = false;
                OnPointerUpEvent();
            }
        }
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (IsActive_)
            {
                IsInsidePointer_ = true;
                OnPointerEnterEvent();
            }
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (IsActive_)
            {
                IsInsidePointer_ = false;
                OnPointerExitEvent();
            }
        }

        protected override void LockAction()
        {
            IsActive_ = false;
        }

        protected override void UnlockAction()
        {
            IsActive_ = false;
        }
    }
}