

using Derevo.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameSceneManualTransition : MonoBehaviour
{
    [SerializeField] private string LevelMapInfoFileName;
    [SerializeField] private string GameSceneName;
    [ContextMenu("TransitTo_GameScene")]
    private void TransitTo_GameScene()
    {
        GameSceneLevelInitialization.LevelName = LevelMapInfoFileName;
        SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }
}