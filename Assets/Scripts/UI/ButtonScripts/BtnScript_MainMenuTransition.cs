
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Derevo.UI.Scripts
{
    public sealed class BtnScript_MainMenuTransition : BtnScript
    {
        [SerializeField] private string MainMenuSceneName;
        protected override void OnPointerDown()
        {
            SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
        }
    }
}