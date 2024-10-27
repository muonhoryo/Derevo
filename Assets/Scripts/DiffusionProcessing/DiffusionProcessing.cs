using System;
using System.Collections;
using System.Collections.Generic;
using Derevo.Level;
using UnityEngine;

namespace Derevo.DiffusionProcessing
{
    public static class DiffusionProcessing
    {
        public struct DiffusionProcessInfo
        {
            public int?[] PreDiffMap;
            public DiffusionProcess[] HandledProcceses;
            public int?[] PostDiffMap;

            public DiffusionProcessInfo(int?[] preDiffMap, DiffusionProcess[] handledProcceses, int?[] postDiffMap)
            {
                PreDiffMap = preDiffMap;
                HandledProcceses = handledProcceses;
                PostDiffMap = postDiffMap;
            }
        }

        public static event Action StartDiffusionEvent=delegate { };
        public static event Action EndDiffusionEvent = delegate { };

        public static DiffusionProcessInfo LastStartedProcessInfo_ { get; private set; }
        public static bool IsInProcess_ { get; private set; } = false;

        public static void StartDiffusion()
        {
            if (IsInProcess_)
                return;
            var preDiffMap = LevelManager.GetValueMap();
            var procs = LevelManager.InitializeDiffusionProcesses();
            foreach (var proc in procs)
                proc.Diffuse();
            var postDiffMap= LevelManager.GetValueMap();
            LastStartedProcessInfo_ = new DiffusionProcessInfo(preDiffMap, procs, postDiffMap);
            IsInProcess_ = true;
            StartDiffusionEvent();
            CoroutinesHandler.Instance_.StartCoroutine(EndDiffusionDelay());
        }
        //DiffusionMap
        public static DiffusionCell GetDiffusionCell(int cellColumn,int cellRow)
        {
            throw new Exception("MRE");
        }

        private static IEnumerator EndDiffusionDelay()
        {
            yield return new WaitForSeconds(GlobalConstsHandler.Instance_.DiffusionProcessTime);
            IsInProcess_ = false;
            EndDiffusionEvent();
        }
    }
}
