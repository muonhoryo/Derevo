

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Derevo.Serialization
{
    public sealed class GameInitialization : MonoBehaviour
    {
        [SerializeField] private string NextSceneName;
        private void Start()
        {
            GlobalConstsSerialization.DeserializeGlobalConsts();
            SceneManager.LoadScene(NextSceneName, LoadSceneMode.Single);
        }
    }
}