

using System;
using Derevo;
using Derevo.Level;
using Derevo.Serialization;
using UnityEngine;

public sealed class SerializationFilesCreatar : MonoBehaviour
{
    [Serializable]
    public struct LevelMapCellEditorInfo
    {
        public bool IsValuable;
        public int Value;
        public ushort ExtendDirection;
    }

    [SerializeField] private GlobalConsts Consts;
    [SerializeField] private LevelMapCellEditorInfo[][] LevelMapInfo;
    [SerializeField] private bool IsFirstCellBottom;
    [SerializeField] private string LevelName;

    [ContextMenu("CreateGlobalConstsFile")]
    public void CreateGlobalConstsFile()
    {
        GlobalConstsSerialization.SerializeLocalGlobalConsts(Consts, GlobalConstsSerialization.GlobalConstsSerializationPath);
        Debug.Log("Created GlobalConsts file at path: " + GlobalConstsSerialization.GlobalConstsSerializationPath);
    }
    [ContextMenu("CreateLevelMapInfo")]
    public void CreateLevelMapInfo()
    {
        if (string.IsNullOrEmpty(LevelName))
            throw new Exception("Missing level name");

        LevelManager.LevelMapInfo info = new LevelManager.LevelMapInfo();
        info.IsFirstCellBottom = IsFirstCellBottom;
        info.CellsInfo=new LevelManager.LevelCellInfo[LevelMapInfo.Length][];
        for(int i=0; i< LevelMapInfo.Length; i++)
        {
            info.CellsInfo[i] = new LevelManager.LevelCellInfo[LevelMapInfo[i].Length];
            for(int j = 0; j < LevelMapInfo[i].Length; j++)
            {
                var cellInfo = LevelMapInfo[i][j];
                if (!cellInfo.IsValuable)
                {
                    info.CellsInfo[i][j] = new LevelManager.BlockCellInfo();
                }
                else if (cellInfo.ExtendDirection == 0)
                {
                    info.CellsInfo[i][j] = new LevelManager.ExtenderCellInfo(cellInfo.Value >= 0 ? cellInfo.Value : 0,
                        cellInfo.ExtendDirection <= ValuableCell.MaxDIffusionDirectionValue ? (ValuableCell.DiffusionDirection)cellInfo.ExtendDirection : ValuableCell.DefaultDirection);
                }
                else
                {
                    info.CellsInfo[i][j] = new LevelManager.ValuableCellInfo(cellInfo.Value >= 0 ? cellInfo.Value : 0);
                }
            }
        }
        GlobalConstsSerialization.SerializeLevelInfo(info,LevelName);
        Debug.Log("Created LevelMapInfo file at path: " + GlobalConstsSerialization.LevelsSerializationDirectory+"\nwith file name: "+LevelName);
    }
}