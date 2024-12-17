


using System.Collections.Generic;
using System.Linq;
using Derevo.DiffusionProcessing;
using Derevo.Level;
using UnityEngine;

public sealed class DiffusionProcessingTest_DiffusionProcesses : MonoBehaviour
{
    private void Awake()
    {
        DiffusionProcessing.StartDiffusionEvent += StartDiffusion;
    }
    private void OnDestroy()
    {
        DiffusionProcessing.StartDiffusionEvent += StartDiffusion;
    }
    private void StartDiffusion()
    {
        foreach(var pr in DiffusionProcessing.LastStartedProcessInfo_.HandledProcceses)
        {
            DiffusionCell[] cells = pr.GetMembers();
            IEnumerable<Vector2Int> first = cells.Select((cel) => cel.CellPosition_);
            IEnumerable<int> second = cells.Select((cel) => (cel.Cell_ is ValuableCell parCel) ? parCel.Value_ : -1);
            IEnumerable<string> info = first.Zip(second, (x, y) => x + "--" + y);
            Debug.Log("pre:\n" + string.Join("\n", info));
        }
    }
}