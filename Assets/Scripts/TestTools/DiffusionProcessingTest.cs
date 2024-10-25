


using Derevo.DiffusionProcessing;
using UnityEngine;

public class DiffusionProcessingText : MonoBehaviour
{
    float StartTime;

    private void Awake()
    {
        DiffusionProcessing.StartDiffusionEvent += StartDiff;
        DiffusionProcessing.EndDiffusionEvent += EndDiff;
    }

    private void StartDiff()
    {
        StartTime = Time.realtimeSinceStartup;
        Debug.Log("startdiff");
        Debug.Log(DiffusionProcessing.LastStartedProcessInfo_.PreDiffMap);
        Debug.Log(DiffusionProcessing.LastStartedProcessInfo_.HandledProcceses);
        Debug.Log(DiffusionProcessing.LastStartedProcessInfo_.PostDiffMap);
    }
    private void EndDiff()
    {
        Debug.Log("enddiff");
        Debug.Log(Time.realtimeSinceStartup - StartTime);
    }

    [ContextMenu("StartTest")]
    public void StartTest()
    {
        DiffusionProcessing.StartDiffusion();
    }
}