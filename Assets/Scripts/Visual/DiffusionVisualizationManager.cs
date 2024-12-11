


using Derevo.Level;
using Derevo.PlayerControl;
using UnityEngine;
using Derevo.DiffusionProcessing;
using System.Collections.Generic;
using Derevo.UI;

namespace Derevo.Visual
{
    public sealed class DiffusionVisualizationManager : MonoBehaviour
    {
        private void Awake()
        {
            GameSceneLevelInitialization.LevelLoadingDoneEvent+= LevelLoadingDone;
        }
        private void LevelLoadingDone()
        {
            GameSceneLevelInitialization.LevelLoadingDoneEvent -= LevelLoadingDone;
            DiffusionProcessing.DiffusionProcessing.StartDiffusionEvent += EndDiffusion;
        }
        private void EndDiffusion()
        {
            foreach(var difProc in DiffusionProcessing.DiffusionProcessing.LastStartedProcessInfo_.HandledProcceses)
            {
                int GetRealCellValue(Vector2Int cellPos)
                {
                    return DiffusionProcessing.DiffusionProcessing.LastStartedProcessInfo_.PostDiffMap[cellPos.x][cellPos.y].Value;
                }
                int GetVisualCellValue(Vector2Int cellPos)
                {
                    return DiffusionProcessing.DiffusionProcessing.LastStartedProcessInfo_.PreDiffMap[cellPos.x][cellPos.y].Value;
                }
                List<DiffusionCell> greaterCells = new List<DiffusionCell> { };
                List<DiffusionCell> lessCells = new List<DiffusionCell> { };
                DiffusionCell[] members=difProc.GetMembers();
                DiffusionCell diffusionTarget;
                DiffusionCell curr;
                ICellContainer curr_visual;
                int realValue;
                int visualValue;
                void DiffusionWithGreaterCurrentCell()
                {
                    diffusionTarget = lessCells[lessCells.Count - 1];
                    int diff =visualValue-GetVisualCellValue(diffusionTarget.CellPosition_);
                    ICellContainer diffusionTarget_visual;
                    diffusionTarget_visual = CellsVisualManager.GetCell(diffusionTarget.CellPosition_.x, diffusionTarget.CellPosition_.y).GetComponent<ICellContainer>();
                    DiffParticlesMovingManager.MoveCell2Cell(difProc, curr_visual, diffusionTarget_visual, GetSpeeds(diff), diff);
                    visualValue -= diff;
                }
                void DiffusionWithLessCurrentCell()
                {
                    diffusionTarget = greaterCells[greaterCells.Count - 1];
                    int diff = GetVisualCellValue(diffusionTarget.CellPosition_)-visualValue;
                    ICellContainer diffusionTarget_visual;
                    diffusionTarget_visual = CellsVisualManager.GetCell(diffusionTarget.CellPosition_.x, diffusionTarget.CellPosition_.y).GetComponent<ICellContainer>();
                    DiffParticlesMovingManager.MoveCell2Cell(difProc,  diffusionTarget_visual, curr_visual, GetSpeeds(diff),  diff);
                    visualValue += diff;
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
                    realValue = GetRealCellValue(curr.CellPosition_);
                    visualValue = GetVisualCellValue(curr.CellPosition_);
                    if (realValue!=visualValue)
                    {
                        curr_visual = CellsVisualManager.GetCell(curr.CellPosition_.x, curr.CellPosition_.y).GetComponent<ICellContainer>();
                        if (visualValue > realValue)
                        {
                            do
                            {
                                if (lessCells.Count > 0)
                                {
                                    DiffusionWithGreaterCurrentCell();
                                }
                                else
                                {
                                    greaterCells.Add(curr);
                                    break;
                                }
                            }
                            while (GetVisualCellValue(curr.CellPosition_) != realValue);
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
                                    lessCells.Add(curr);
                                    break;
                                }
                            }
                            while (GetVisualCellValue(curr.CellPosition_) != realValue);
                        }
                    }
                }
            }
        }
    }
}