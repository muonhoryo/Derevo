

using Derevo.Level;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Derevo.UI.Scripts
{
    public class BtnScript_LevelTransition : BtnScript
    {
        [SerializeField] private string GameSceneName;
        [SerializeField] private string LevelName;

        protected override void OnPointerDown()
        {
            TransitionAction();
        }

        protected void TransitionAction()
        {
            GameSceneLevelInitialization.LevelName = LevelName;
            SceneManager.LoadScene(GameSceneName);
        }
    }
}