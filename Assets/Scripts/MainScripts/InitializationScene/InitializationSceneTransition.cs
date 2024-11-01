

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Derevo.Serialization
{
    public sealed class InitializationSceneTransition : MonoBehaviour
    {
        [SerializeField] private string MainMenuSceneName;
        private void Start()
        {
            GlobalConstsSerialization.DeserializeGlobalConsts();
            SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
        }
    }
}