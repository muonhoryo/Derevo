

using System.Collections;
using Derevo.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Derevo.UI.Scripts
{
    public sealed class BtnScript_DelayedLevelTransition : BtnScript_LevelTransition
    {
        protected override void OnPointerDown()
        {
            StartCoroutine(DelayedTransition());
        }
        private IEnumerator DelayedTransition()
        {
            yield return new WaitForSeconds(GlobalConstsHandler.Instance_.LevelTransitionDelay);
            TransitionAction();
        }
    }
}