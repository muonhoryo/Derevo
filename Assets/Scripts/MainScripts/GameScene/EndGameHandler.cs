

using Derevo.Level;
using UnityEngine;

namespace Derevo 
{
    public sealed class EndGameHandler : MonoBehaviour
    {
        [SerializeField] private GameObject WinMessage;
        [SerializeField] private GameObject LoseMessage;

        private void Awake()
        {
            DiffusionProcessing.DiffusionProcessing.EndDiffusionEvent += EndDiffusionAction;
        }
        private void EndDiffusionAction()
        {
            if (DiffusionParticlesManager.RemainedParticlesCount_ == 0)
            {
                PlayerControl.PlayerControlLocker.Lock();
                DiffusionProcessing.DiffusionProcessing.EndDiffusionEvent-= EndDiffusionAction;
                if (CalculatePlayerState())
                    Win();
                else
                    Lose();
            }
        }
        private bool CalculatePlayerState()
        {
            int value=-1;
            ValuableCell cell;
            for(int i = 0; i < LevelManager.Width_; i++)
            {
                for(int j = 0; j < LevelManager.MaxHeight_; j++)
                {
                    cell=LevelManager.GetCell(i, j) as ValuableCell;
                    if (cell != null)
                    {
                        if (value < 0)
                        {
                            value = cell.Value_;
                        }
                        else
                        {
                            if (cell.Value_ != value)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        private void Win()
        {
            WinMessage.SetActive(true);
        }
        private void Lose()
        {
            LoseMessage.SetActive(true); 
        }
    }
}