


using System.Collections;
using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;

namespace Derevo.UI.Scripts
{
    public sealed class AncSldScript_MoveParticles : MonoBehaviour
    {
        [SerializeField] private AnchorSlider Slider;

        private bool IsIncreasing;
        private float MovingParticlesTimeDelay;
        private Coroutine CurrentCoroutine=null;

        private void Awake()
        {
            Slider.ChangeValueEvent += ValueChanged;
            Slider.ResetSliderEvent += ResetSlider;
        }
        private void ValueChanged(float value)
        {
            int countPTick = (int)value;
            if (countPTick > 0)
                Increase(countPTick);
            else if (countPTick < 0)
                Decrease(-countPTick);
            else
                ResetSlider();
        }
        private void Increase(int countPTick)
        {
            if (CurrentCoroutine != null && !IsIncreasing)
                StopCoroutine(CurrentCoroutine);
            if (DiffusionParticlesManager.RemainedParticlesCount_ == 0)
                return;

            MovingParticlesTimeDelay = GlobalConstsHandler.Instance_.ParticlesChanging_MinDelay / countPTick;
            
            if (CurrentCoroutine == null)
            {
                CurrentCoroutine = StartCoroutine(Increasing());
                    IsIncreasing = true;
            }
        }
        private IEnumerator Increasing()
        {
            while (true)
            {
                UnfixedCellsControl.IncreaseValue();
                yield return new WaitForSeconds(MovingParticlesTimeDelay);
                if (DiffusionParticlesManager.RemainedParticlesCount_ == 0)
                    ResetSlider();
            }
        }
        private void Decrease(int countPTick)
        {
            if (CurrentCoroutine != null && IsIncreasing)
                StopCoroutine(CurrentCoroutine);

            MovingParticlesTimeDelay = GlobalConstsHandler.Instance_.ParticlesChanging_MinDelay / countPTick;
            if (CurrentCoroutine == null)
            {
                CurrentCoroutine = StartCoroutine(Decreasing());
                IsIncreasing = false;
            }
        }
        private IEnumerator Decreasing()
        {
            while (true)
            {
                UnfixedCellsControl.DecreaseValue();
                yield return new WaitForSeconds(MovingParticlesTimeDelay);
                if (UnfixedCellsManager.Target_==null)
                    ResetSlider();
            }
        }

        private void ResetSlider()
        {
            if (CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);
        }
    }
}