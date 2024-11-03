

using Derevo.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Derevo.GUI
{
    public sealed class CellParticleCounter : MonoBehaviour
    {
        private ValuableCell Target;
        [SerializeField] private Text CounterText;

        public void Initialize(ValuableCell target)
        {
            Target = target;
            ChangeCount();
        }
        private void Awake()
        {
            LevelManager.ChangeCellValueEvent += ChangeValueAction;
        }
        private void OnDestroy()
        {
            Destroy(CounterText);
            LevelManager.ChangeCellValueEvent-= ChangeValueAction;
        }
        private void ChangeValueAction(LevelManager.ChangeCellValueEventInfo info)
        {
            if (info.ChangedCell == Target)
            {
                ChangeCount();
            }
        }
        private void ChangeCount()
        {
            CounterText.text = Target.Value_.ToString();
        }
    }
}