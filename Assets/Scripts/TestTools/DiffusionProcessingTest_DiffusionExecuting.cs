

using System.Text;
using Derevo.DiffusionProcessing;
using Derevo.Level;
using UnityEngine;

public class DiffusionProcessingTest_DiffusionExecuting:MonoBehaviour
{
    private LevelManager.LevelMapInfo Map = new();
    private void Awake()
    {
        const int height=4;
        LevelManager.LevelCellInfo[][] mapCells = new LevelManager.LevelCellInfo[][]
        {
            new LevelManager.LevelCellInfo[height]
            {
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ValuableCellInfo(0)
            },
            new LevelManager.LevelCellInfo[height]
            {
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ExtenderCellInfo(0,ValuableCell.DiffusionDirection.Right),
                new LevelManager.ExtenderCellInfo(9,ValuableCell.DiffusionDirection.Bottom),
                new LevelManager.ValuableCellInfo(0)
            },
            new LevelManager.LevelCellInfo[height]
            {
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ExtenderCellInfo(0,ValuableCell.DiffusionDirection.Top),
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ValuableCellInfo(0)
            },
            new LevelManager.LevelCellInfo[height]
            {
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ValuableCellInfo(0),
                new LevelManager.ValuableCellInfo(0)
            }
        };
        Map.CellsInfo = mapCells;
        Map.Width = mapCells.Length;
        Map.Height= height;

        LevelManager.InitializeMapEvent += () => Debug.Log("initialize new map");
        LevelManager.InitializeMapEvent += PrintLevelMap;
        LevelManager.ChangeCellValueEvent += (info) =>
        {
            Debug.Log(info.ChangedCell.GetType().ToString() + '(' + info.Column + ", " + info.Row + "): " +
                info.OldValue + "---->" + info.NewValue);
        };
        LevelManager.ChangeCellDiffDirectionEvent += (info) =>
        {
            Debug.Log(info.ChangedCell.GetType().ToString() + '(' + info.Column + ", " + info.Row + "): " +
                info.OldDirection + "---->" + info.NewDirection);
        };
        DiffusionProcessing.StartDiffusionEvent += PrintDiffusionInfo;

        LevelManager.InitializeLevel(Map);
        LevelManager.SetCellDiffusionDirection(ValuableCell.DiffusionDirection.Top, 2, 0);
        PrintLevelMap();
        DiffusionProcessing.StartDiffusion();
        PrintLevelMap();
    }

    private void PrintLevelMap()
    {
        StringBuilder mainStr = new StringBuilder();
        StringBuilder valueStr = new StringBuilder();
        StringBuilder dirStr=new StringBuilder();
        ValuableCell cell;
        for(int i = LevelManager.LevelMapSize_.y-1; i >=0 ; i--)
        {
            for(int j = 0; j < LevelManager.LevelMapSize_.x; j++)
            {
                cell = LevelManager.GetCell(j, i) as ValuableCell;
                if (cell == null)
                {
                    valueStr.Append(" X |");
                    dirStr.Append(" X|");
                    continue;
                }
                else
                    valueStr.Append(cell.Value_.ToString()+"|");

                string appStr;
                ValuableCell.DiffusionDirection direction = cell.DiffusionDirection_;
                if(cell is ExtenderCell parCell)
                {
                    direction = direction | parCell.ExtendDirection_;
                }
                switch (direction)
                {
                    case ValuableCell.DiffusionDirection.Right:
                        appStr = "->|";
                        break;
                    case ValuableCell.DiffusionDirection.Top:
                        appStr = " ^|";
                        break;
                    case ValuableCell.DiffusionDirection.Left:
                        appStr = "<-|";
                        break;
                    case ValuableCell.DiffusionDirection.Bottom:
                        appStr = " v|";
                        break;
                    default:
                        appStr = " X|";
                        break;
                }
                dirStr.Append(appStr);
            }
            mainStr.Append(valueStr);
            mainStr.Append("_____");
            mainStr.Append(dirStr);
            mainStr.Append("\n");
            valueStr.Clear();
            dirStr.Clear();
        }
        Debug.Log(mainStr);
    }
    private void PrintDiffusionInfo()
    {
        StringBuilder str = new StringBuilder("Start Diffusion\n\n");
        void PrintValueMap(int?[][] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    str.Append(map[i][j] + '|');
                }
                str.Append('\n');
            }
        }
        str.Append("PreDiffMap:\n");
        PrintValueMap(DiffusionProcessing.LastStartedProcessInfo_.PreDiffMap);
        str.Append("\nPostDiffMap:\n");
        PrintValueMap(DiffusionProcessing.LastStartedProcessInfo_.PostDiffMap);
        str.Append("\nDiffusionMap:\n");
        for(int i = 0; i < DiffusionProcessing.LastStartedProcessInfo_.DiffusionMap.Length; i++)
        {
            for(int j = 0; j < DiffusionProcessing.LastStartedProcessInfo_.DiffusionMap[i].Length; j++)
            {
                str.AppendLine('(' + i + ", " + j + "): " + DiffusionProcessing.LastStartedProcessInfo_.DiffusionMap[i][j].CellPosition_ +
                    DiffusionProcessing.LastStartedProcessInfo_.DiffusionMap[i][j].Cell_.GetType().ToString());
            }
        }
        Debug.Log(str);
    }
}