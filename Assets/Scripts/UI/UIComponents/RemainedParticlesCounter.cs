

using Derevo.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Derevo.UI
{
    public sealed class RemainedParticlesCounter : MonoBehaviour
    {
        [SerializeField] private Text CounterText;

        private void Awake()
        {
            DiffusionParticlesManager.RemainedParticlesChangedEvent += ParticlesCountChangedAction;
        }
        private void OnDestroy()
        {
            DiffusionParticlesManager.RemainedParticlesChangedEvent-= ParticlesCountChangedAction;
        }
        private void ParticlesCountChangedAction(DiffusionParticlesManager.RemainedParticlesChangedEventInfo info)
        {
            UpdateCounterText(info.NewValue);
        }
        private void UpdateCounterText(int newCount)
        {
            CounterText.text = newCount.ToString();
        }
    }
}