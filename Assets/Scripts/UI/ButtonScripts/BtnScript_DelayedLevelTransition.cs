

using System.Collections;
using Derevo.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Derevo.UI.Scripts
{
    public sealed class BtnScript_DelayedLevelTransition : BtnScript
    {
        [SerializeField] private string GameSceneName;
        [SerializeField] private string LevelName;

        protected override void OnPointerDown()
        {
            StartCoroutine(DelayedTransition());
        }
        private IEnumerator DelayedTransition()
        {
            yield return new WaitForSeconds(GlobalConstsHandler.Instance_.LevelTransitionDelay);
            GameSceneLevelInitialization.LevelName = LevelName;
            SceneManager.LoadScene(GameSceneName);
        }
    }
}