

using System.Text;
using Derevo.DiffusionProcessing;
using Derevo.Level;
using UnityEngine;

public class DiffusionProcessingTest_DiffusionExecuting:MonoBehaviour
{
    private LevelManager.LevelMapInfo Map = new();
    private void Awake()
    {
        const int maxHeight=4;
        LevelManager.LevelMapCellInfo[][] mapCells = new LevelManager.LevelMapCellInfo[][]
        {
            new LevelManager.LevelMapCellInfo[maxHeight]
            {
                new LevelManager.LevelMapCellInfo(true,0,0),
                new LevelManager.LevelMapCellInfo(true,0,0),
                new LevelManager.LevelMapCellInfo(true,0,0),
                new LevelManager.LevelMapCellInfo(true,0,0)
            },
            new LevelManager.LevelMapCellInfo[maxHeight]
            {
                new LevelManager.LevelMapCellInfo(true,0,0),
                new LevelManager.LevelMapCellInfo(true,0,(ushort)ValuableCell.DiffusionDirection.BottomRight),
                new LevelManager.LevelMapCellInfo(true,9,(ushort)ValuableCell.DiffusionDirection.Bottom),
                new LevelManager.LevelMapCellInfo (true, 0, 0)
            },
            new LevelManager.LevelMapCellInfo[3]
            {
                new LevelManager.LevelMapCellInfo(true,0,0),
                new LevelManager.LevelMapCellInfo(true,0,(ushort)ValuableCell.DiffusionDirection.Top),
                new LevelManager.LevelMapCellInfo (true, 0, 0)
            },
            new LevelManager.LevelMapCellInfo[maxHeight]
            {
                new LevelManager.LevelMapCellInfo (true, 0, 0),
                new LevelManager.LevelMapCellInfo (true, 0, 0),
                new LevelManager.LevelMapCellInfo (true, 0, 0),
                new LevelManager.LevelMapCellInfo (false, 0, 0)
            }
        };
        Map.ColumnsInfo = LevelManager.LevelMapColumnInfo.ConvertMapInfoToColumnsInfo(mapCells);
        Map.IsFirstCellBottom = false;

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
        LevelManager.TrySetCellDiffusionDirection(ValuableCell.DiffusionDirection.Top, 2, 0);
        PrintLevelMap();
        DiffusionProcessing.StartDiffusion();
        PrintLevelMap();
    }

    public static void PrintLevelMap()
    {
        StringBuilder mainStr = new StringBuilder();
        StringBuilder valueStr = new StringBuilder();
        StringBuilder dirStr=new StringBuilder();
        ValuableCell cell;
        for(int i = LevelManager.MaxHeight_-1; i >=0 ; i--)
        {
            for (int w = LevelManager.IsFirstCellBottom_ ? 1 : 0; w < LevelManager.Width_; w += 2)
            {
                if (LevelManager.IsFirstCellBottom_)
                {
                    valueStr.Append("___");
                    dirStr.Append("___");
                }
                cell = LevelManager.GetCell(w, i) as ValuableCell;
                if (cell == null)
                {
                    valueStr.Append(" X |");
                    dirStr.Append(" X|");
                    continue;
                }
                else
                    valueStr.Append(cell.Value_.ToString() + "|");

                string appStr;
                ValuableCell.DiffusionDirection direction = cell.DiffusionDirection_;
                if (cell is ExtenderCell parCell)
                {
                    direction = direction | parCell.ExtendDirection_;
                }
                switch (direction)
                {
                    case ValuableCell.DiffusionDirection.TopRight:
                        appStr = "^>|";
                        break;
                    case ValuableCell.DiffusionDirection.Top:
                        appStr = " ^|";
                        break;
                    case ValuableCell.DiffusionDirection.TopLeft:
                        appStr = "<^|";
                        break;
                    case ValuableCell.DiffusionDirection.BottomLeft:
                        appStr = "<v|";
                        break;
                    case ValuableCell.DiffusionDirection.Bottom:
                        appStr = " v|";
                        break;
                    case ValuableCell.DiffusionDirection.BottomRight:
                        appStr = "v>|";
                        break;
                    default:
                        appStr = " X|";
                        break;
                }
                dirStr.Append(appStr);
                if (!LevelManager.IsFirstCellBottom_)
                {
                    valueStr.Append("___");
                    dirStr.Append("___");
                }
            }
            mainStr.Append(valueStr);
            mainStr.Append("_____");
            mainStr.Append(dirStr);
            mainStr.Append("\n");
            valueStr.Clear();
            dirStr.Clear();
            for(int w = LevelManager.IsFirstCellBottom_ ? 0 : 1; w < LevelManager.Width_; w += 2)
            {
                if (!LevelManager.IsFirstCellBottom_)
                {
                    valueStr.Append("___");
                    dirStr.Append("___");
                }
                cell = LevelManager.GetCell(w, i) as ValuableCell;
                if (cell == null)
                {
                    valueStr.Append(" X |");
                    dirStr.Append(" X|");
                    continue;
                }
                else
                    valueStr.Append(cell.Value_.ToString() + "|");

                string appStr;
                ValuableCell.DiffusionDirection direction = cell.DiffusionDirection_;
                if (cell is ExtenderCell parCell)
                {
                    direction = direction | parCell.ExtendDirection_;
                }
                switch (direction)
                {
                    case ValuableCell.DiffusionDirection.TopRight:
                        appStr = "^>|";
                        break;
                    case ValuableCell.DiffusionDirection.Top:
                        appStr = " ^|";
                        break;
                    case ValuableCell.DiffusionDirection.TopLeft:
                        appStr = "<^|";
                        break;
                    case ValuableCell.DiffusionDirection.BottomLeft:
                        appStr = "<v|";
                        break;
                    case ValuableCell.DiffusionDirection.Bottom:
                        appStr = " v|";
                        break;
                    case ValuableCell.DiffusionDirection.BottomRight:
                        appStr = "v>|";
                        break;
                    default:
                        appStr = " X|";
                        break;
                }
                dirStr.Append(appStr);
                if (LevelManager.IsFirstCellBottom_)
                {
                    valueStr.Append("___");
                    dirStr.Append("___");
                }
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