

using Derevo;
using Derevo.Serialization;
using UnityEngine;

public sealed class SerializationFilesCreatar : MonoBehaviour
{
    [SerializeField] private GlobalConsts Consts;

    [ContextMenu("CreateGlobalConstsFile")]
    public void CreateGlobalConstsFile()
    {
        GlobalConstsSerialization.SerializeLocalGlobalConsts(Consts, GlobalConstsSerialization.GlobalConstsSerializationPath);
        Debug.Log("Created GlobalConsts file at path: " + GlobalConstsSerialization.GlobalConstsSerializationPath);
    }
}