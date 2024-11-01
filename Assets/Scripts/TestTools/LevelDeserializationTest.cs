

using Derevo.DiffusionProcessing;
using Derevo.Level;
using UnityEngine;

public sealed class LevelDeserializationTest : MonoBehaviour
{
    private void Awake()
    {
        GameSceneLevelInitialization.LevelLoadingDoneEvent += LevelLoadingDoneEvent;
    }
    private void LevelLoadingDoneEvent()
    {
        DiffusionProcessingTest_DiffusionExecuting.PrintLevelMap();
        DiffusionProcessing.StartDiffusion();
        DiffusionProcessingTest_DiffusionExecuting.PrintLevelMap();
    }
}