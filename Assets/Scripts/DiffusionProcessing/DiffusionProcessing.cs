using System;
using System.Collections;
using System.Collections.Generic;
using Derevo.Level;
using UnityEngine;

namespace Derevo.DiffusionProcessing
{
    public static class DiffusionProcessing
    {
        public sealed class DiffusionProcessInfo
        {
            public int?[][] PreDiffMap;
            public DiffusionProcess[] HandledProcceses;
            public DiffusionCell[][] DiffusionMap;
            public int?[][] PostDiffMap;
        }

        public static event Action StartDiffusionEvent=delegate { };
        public static event Action EndDiffusionEvent = delegate { };

        public static DiffusionProcessInfo LastStartedProcessInfo_ { get; private set; }
        public static bool IsInProcess_ { get; private set; } = false;

        public static void StartDiffusion()
        {
            if (IsInProcess_)
                return;

            LastStartedProcessInfo_=new DiffusionProcessInfo();
            LastStartedProcessInfo_.PreDiffMap= LevelManager.GetValueMap();
            LastStartedProcessInfo_.DiffusionMap = LevelManager.GetDiffusionMap();
            LastStartedProcessInfo_.HandledProcceses = LevelManager.InitializeDiffusionProcesses();
            foreach (var proc in LastStartedProcessInfo_.HandledProcceses)
                proc.Diffuse();
            LevelManager.ResetValuableCellsDirections();
            LastStartedProcessInfo_.PostDiffMap = LevelManager.GetValueMap();
            IsInProcess_ = true;
            StartDiffusionEvent();
            CoroutinesHandler.Instance_.StartCoroutine(EndDiffusionDelay());
        }
        //DiffusionMap
        public static DiffusionCell GetDiffusionCell(int cellColumn,int cellRow)
        {
            return LastStartedProcessInfo_.DiffusionMap[cellColumn][ cellRow];
        }

        private static IEnumerator EndDiffusionDelay()
        {
            yield return new WaitForSeconds(GlobalConstsHandler.Instance_.DiffusionProcessTime);
            IsInProcess_ = false;
            EndDiffusionEvent();
        }
    }
}
