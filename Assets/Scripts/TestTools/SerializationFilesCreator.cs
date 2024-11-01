
using System;
using Derevo;
using Derevo.Level;
using Derevo.Serialization;
using UnityEngine;

public sealed class SerializationFilesCreator : MonoBehaviour
{
    [SerializeField] private GlobalConsts Consts;
    [SerializeField] private LevelManager.LevelMapInfo LevelMapInfo;
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

        LevelMapSerialization.SerializeLevelInfo(LevelMapInfo, LevelName);
        Debug.Log("Created LevelMapInfo file at path: " + LevelMapSerialization.LevelsSerializationDirectory + "\nwith file name: " + LevelName);
    }
}