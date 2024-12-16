

using MuonhoryoLibrary.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using Derevo.PlayerControl;

namespace Derevo.UI
{
    public sealed class RadialSelector : PlayerControlElement, IPointerDownHandler
    {
        public event Action<float,int> ChangeValueEvent = delegate { };

        [SerializeField] private float[] Steps;
        [SerializeField] private GameObject Handler;

        public int CurrentDirectionIndex_ { get; private set; } = -1;
        public bool IsHolding_ { get; private set; } = false;
        public float Value_ { get; private set; }

        protected override void AwakeAction()
        {
            Steps = Steps.OrderBy(x => x).ToArray();
        }
        private void Start()
        {
            ChangeValueByIndex(0);
        }
        private void Update()
        {
            if (IsHolding_)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    OnPointerUp();
                    return;
                }

                OnHold(Input.mousePosition);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsHolding_ = true;
            OnHold(eventData.position);
        }
        private void OnHold(Vector2 mouseScreenPos)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 dir = mousePos - (Vector2)transform.position;
            float angle = dir.AngleFromDirection();
            if(angle == Steps[Steps.Length - 1])
            {
                ChangeValueByIndex(Steps.Length - 1);
                return;
            }
            if (angle < Steps[0]||
                angle > Steps[Steps.Length-1])
            {
                if ((Steps[0] - angle + 360) % 360 < (angle- Steps[Steps.Length - 1] + 360) % 360)
                {
                    ChangeValueByIndex(0);
                }
                else
                {
                    ChangeValueByIndex(Steps.Length - 1);
                }
                return;
            }
            for (int i = 0; i < Steps.Length - 1; i++)
            {
                if (angle == Steps[i])
                {
                    ChangeValueByIndex(i);
                    return;
                }   
                else if (Steps[i + 1]>angle)
                {
                    if (angle - Steps[i] <
                        Steps[i + 1] - angle)
                    { 
                        ChangeValueByIndex(i);
                    }
                    else
                    {
                        ChangeValueByIndex(i + 1);
                    }
                    return;
                }
            }
            throw new System.Exception("Some problem with steps array.");
        }
        public void ChangeValueByIndex(int index)
        {
            if (CurrentDirectionIndex_ == index)
                return;

            CurrentDirectionIndex_ = index;
            Handler.transform.localRotation =Quaternion.Euler(
                Handler.transform.localRotation.x,
                Handler.transform.localRotation.y,
                Steps[CurrentDirectionIndex_]);
            Value_ = Steps[CurrentDirectionIndex_];
            ChangeValueEvent(Value_,CurrentDirectionIndex_);
        }
        private void OnPointerUp()
        {
            IsHolding_ = false;
        }

        protected override void LockAction()
        {
            ChangeValueByIndex(0);
            OnPointerUp();
            enabled = false;
        }
        protected override void UnlockAction()
        {
            enabled = true;
        }
    }
}