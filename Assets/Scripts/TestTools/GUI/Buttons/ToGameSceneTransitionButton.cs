

using Derevo.Level;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Derevo.GUI
{
    public sealed class ToGameSceneTransitionButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private string GameSceneName;
        [SerializeField] private string LevelName;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Transit();
        }

        [ContextMenu("ManualTransit")]
        private void Transit()
        {
            GameSceneLevelInitialization.LevelName = LevelName;
            SceneManager.LoadScene(GameSceneName);
        }
    }
}