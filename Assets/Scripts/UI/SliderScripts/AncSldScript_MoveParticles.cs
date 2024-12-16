


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
        private int CountPerTick;
        private Coroutine CurrentCoroutine=null;

        private void Awake()
        {
            Slider.ChangeValueEvent += ValueChanged;
            Slider.ResetSliderEvent += StopCurrentCoroutine;
        }
        private void ValueChanged(float value)
        {
            if (UnfixedCellsManager.Target_ == null)
                return;

            int countPTick = (int)value;
            if (countPTick > 0)
                Increase(countPTick);
            else if (countPTick < 0)
                Decrease(-countPTick);
            else
                StopCurrentCoroutine();
        }

        private void Increase(int countPTick)
        {
            if (DiffusionParticlesManager.RemainedParticlesCount_ == 0)
                return;

            float newTimeDelay= GlobalConstsHandler.Instance_.ParticlesChanging_MinDelay / countPTick;

            void StartIncCoroutine()
            {
                MovingParticlesTimeDelay = newTimeDelay;
                CountPerTick = countPTick;
                CurrentCoroutine = StartCoroutine(Increasing());
                IsIncreasing = true;
            }

            if (CurrentCoroutine != null)
            {
                if (!IsIncreasing || CountPerTick != countPTick)
                {
                    InternalStopCurrentCoroutine();
                    StartIncCoroutine();
                }
            }
            else
            {
                StartIncCoroutine();
            }

        }
        private IEnumerator Increasing()
        {
            while (true)
            {
                UnfixedCellsControl.IncreaseValue();
                yield return new WaitForSeconds(MovingParticlesTimeDelay);
                if (DiffusionParticlesManager.RemainedParticlesCount_ == 0)
                    InternalStopCurrentCoroutine();
            }
        }
        private void Decrease(int countPTick)
        {
            float newTimeDelay= GlobalConstsHandler.Instance_.ParticlesChanging_MinDelay / countPTick;

            void StartDecCoroutine()
            {
                MovingParticlesTimeDelay = newTimeDelay;
                CountPerTick= countPTick;
                CurrentCoroutine = StartCoroutine(Decreasing());
                IsIncreasing = false;
            }
            if (CurrentCoroutine != null)
            {
                if (IsIncreasing || CountPerTick != countPTick)
                {
                    InternalStopCurrentCoroutine();
                    StartDecCoroutine();
                }
            }
            else
            {
                StartDecCoroutine();
            }
        }
        private IEnumerator Decreasing()
        {
            while (true)
            {
                UnfixedCellsControl.DecreaseValue();
                yield return new WaitForSeconds(MovingParticlesTimeDelay);
                if (UnfixedCellsManager.Target_ == null)
                    InternalStopCurrentCoroutine();
            }
        }

        private void StopCurrentCoroutine()
        {
            if (CurrentCoroutine != null)
            {
                InternalStopCurrentCoroutine();
            }
        }
        private void InternalStopCurrentCoroutine()
        {
            StopCoroutine(CurrentCoroutine);
            CurrentCoroutine = null;
        }
    }
}