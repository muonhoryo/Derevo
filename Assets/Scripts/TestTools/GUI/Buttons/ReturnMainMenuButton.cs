

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Derevo.GUI
{
    public sealed class ReturnMainMenuButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private string MainMenuSceneName;

        public void OnPointerDown(PointerEventData eventData)
        {
            SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
        }
    }
}