


using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;
using Derevo.DiffusionProcessing;
using System.Collections.Generic;
using Derevo.UI;
using System.Linq;
using UnityEngine.AI;

namespace Derevo.Visual
{
    public sealed class DiffusionVisualizationManager : MonoBehaviour
    {
        private sealed class DiffusionCellChangingInfo
        {
            public int VisualValue;
            public readonly int RealValue;
            public readonly Vector2Int CellPosition;

            private DiffusionCellChangingInfo() { }
            public DiffusionCellChangingInfo(int visualValue, int realValue, Vector2Int cellPosition)
            {
                VisualValue = visualValue;
                RealValue = realValue;
                CellPosition = cellPosition;
            }
            public DiffusionCellChangingInfo(DiffusionCell owner)
            {
                VisualValue = CellsVisualManager.GetCell(owner.CellPosition_.x, owner.CellPosition_.y).GetComponent<IParticlesContainer>().UploadedParticlesCount_;
                RealValue= DiffusionProcessing.DiffusionProcessing.LastStartedProcessInfo_.PostDiffMap[owner.CellPosition_.x][owner.CellPosition_.y].Value;
                CellPosition = owner.CellPosition_;
            }
        }

        [SerializeField] private RemParticlesVisualIndicator RemParticlesVisualIndicator;

        private void Awake()
        {
            GameSceneLevelInitialization.LevelLoadingDoneEvent+= LevelLoadingDone;
        }
        private void OnDestroy()
        {
            DiffusionProcessing.DiffusionProcessing.StartDiffusionEvent -= StartDiffusion;
        }
        private void LevelLoadingDone()
        {
            GameSceneLevelInitialization.LevelLoadingDoneEvent -= LevelLoadingDone;
            DiffusionProcessing.DiffusionProcessing.StartDiffusionEvent += StartDiffusion;
            LevelManager.ChangeCellValueEvent += ChangeValueCell;
        }

        private void StartDiffusion()
        {
            foreach(var difProc in DiffusionProcessing.DiffusionProcessing.LastStartedProcessInfo_.HandledProcceses)
            {
                int GetRealCellValue(Vector2Int cellPos)
                {
                    return DiffusionProcessing.DiffusionProcessing.LastStartedProcessInfo_.PostDiffMap[cellPos.x][cellPos.y].Value;
                }
                int GetVisualCellValue(Vector2Int cellPos)
                {
                    return CellsVisualManager.GetCell(cellPos.x, cellPos.y).GetComponent<IParticlesContainer>().UploadedParticlesCount_;
                }
                List<DiffusionCellChangingInfo> greaterCells = new List<DiffusionCellChangingInfo> { };
                List<DiffusionCellChangingInfo> lessCells = new List<DiffusionCellChangingInfo> { };
                DiffusionCell[] members=difProc.GetMembers();
                DiffusionCellChangingInfo diffusionTarget;
                DiffusionCell curr;
                ICellContainer curr_visual;
                int curr_realValue;
                int curr_visualValue;
                void DiffusionWithGreaterCurrentCell()
                {
                    diffusionTarget = lessCells[lessCells.Count - 1];
                    int target_visualValue = diffusionTarget.VisualValue;
                    int target_realValue = diffusionTarget.RealValue;
                    int targetDiff =  target_realValue- target_visualValue;
                    int currDiff = curr_visualValue - curr_realValue;
                    int movedParticles;
                    if (targetDiff >= currDiff)
                    {
                        movedParticles = targetDiff;
                        lessCells.RemoveAt(lessCells.Count - 1);
                    }
                    else
                    {
                        movedParticles = currDiff;
                        lessCells[lessCells.Count - 1].VisualValue += currDiff;
                    }
                    ICellContainer diffusionTarget_visual;
                    diffusionTarget_visual = CellsVisualManager.GetCell(diffusionTarget.CellPosition.x, diffusionTarget.CellPosition.y).GetComponent<ICellContainer>();
                    float[] speeds = GetSpeeds(movedParticles);
                    DiffParticlesMovingManager.MoveCell2Cell(difProc, curr_visual, diffusionTarget_visual, speeds, movedParticles);
                    curr_visualValue -= movedParticles;
                }
                void DiffusionWithLessCurrentCell()
                {
                    diffusionTarget = greaterCells[greaterCells.Count - 1];
                    int target_visualValue = diffusionTarget.VisualValue;
                    int target_realValue = diffusionTarget.RealValue;
                    int targetDiff = target_visualValue - target_realValue;
                    int currDiff = curr_realValue-curr_visualValue;
                    int movedParticles;
                    if (targetDiff <= currDiff)
                    {
                        movedParticles = targetDiff;
                        greaterCells.RemoveAt(greaterCells.Count - 1);
                    }
                    else
                    {
                        movedParticles = currDiff;
                        greaterCells[greaterCells.Count - 1].VisualValue -= currDiff;
                    }
                    ICellContainer diffusionTarget_visual;
                    diffusionTarget_visual = CellsVisualManager.GetCell(diffusionTarget.CellPosition.x, diffusionTarget.CellPosition.y).GetComponent<ICellContainer>();
                    float[] speeds = GetSpeeds(movedParticles);
                    DiffParticlesMovingManager.MoveCell2Cell(difProc,  diffusionTarget_visual, curr_visual, speeds, movedParticles);
                    curr_visualValue += movedParticles;
                }
                float[] GetSpeeds(int particlesCount)
                {
                    float[] speeds = new float[particlesCount];
                    float min = GlobalConstsHandler.Instance_.ParticlesDiffusionSpeedMin;
                    float max = min + GlobalConstsHandler.Instance_.ParticlesDiffusionSpeedDispersion;
                    for (int i = 0; i < particlesCount; i++)
                    {
                        speeds[i] = Random.Range(min, max);
                    }
                    return speeds;
                }
                for(int i = members.Length - 1; i >= 0; i--)
                {
                    curr = members[i];
                    curr_realValue = GetRealCellValue(curr.CellPosition_);
                    curr_visualValue = GetVisualCellValue(curr.CellPosition_);

                    if (curr_realValue!=curr_visualValue)
                    {
                        curr_visual = CellsVisualManager.GetCell(curr.CellPosition_.x, curr.CellPosition_.y).GetComponent<ICellContainer>();
                        if (curr_visualValue > curr_realValue)
                        {
                            do
                            {
                                if (lessCells.Count > 0)
                                {
                                    DiffusionWithGreaterCurrentCell();
                                }
                                else
                                {
                                    greaterCells.Add(new DiffusionCellChangingInfo(curr));
                                    break;
                                }
                            }
                            while (curr_visualValue != curr_realValue);
                        }
                        else
                        {
                            do
                            {
                                if (greaterCells.Count > 0)
                                {
                                    DiffusionWithLessCurrentCell();
                                }
                                else
                                {
                                    lessCells.Add(new DiffusionCellChangingInfo(curr));
                                    break;
                                }
                            }
                            while (curr_visualValue != curr_realValue);
                        }
                    }
                }
            }
        }
        private void ChangeValueCell(LevelManager.ChangeCellValueEventInfo info)
        {
            if (DiffusionProcessing.DiffusionProcessing.IsInProcess_)
                return;

            int diff;
            IParticlesContainer cell= CellsVisualManager.GetCell(info.Column, info.Row).GetComponent<IParticlesContainer>();
            IParticlesContainer origin;
            IParticlesContainer destination;
            float speed = GlobalConstsHandler.Instance_.ParticlesDiffusionSpeedMin + GlobalConstsHandler.Instance_.ParticlesDiffusionSpeedDispersion;
            if (info.NewValue < info.OldValue)
            {
                diff = info.OldValue - info.NewValue;
                origin = cell;
                destination = RemParticlesVisualIndicator;
            }
            else
            {
                diff = info.NewValue - info.OldValue;
                origin = RemParticlesVisualIndicator;
                destination = cell;
            }
            float[] speeds=Enumerable.Repeat(speed, diff).ToArray();
            DiffParticlesMovingManager.MoveDirectly(origin,destination,speeds, diff); 
        }
    }
}