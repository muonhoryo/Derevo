

using System.IO;
using UnityEngine;

namespace Derevo.Serialization 
{
    public static class GlobalConstsSerialization
    {
        public const string GlobalConstsSerializationPath =
#if UNITY_EDITOR
            "Assets/Scripts/Editor/GlobalConsts.json";
#else
            "GlobalConsts.json";
#endif

        public static void SerializeGlobalConsts()
        {
            SerializeLocalGlobalConsts(GlobalConstsHandler.Instance_, GlobalConstsSerializationPath);
        }
        public static void DeserializeGlobalConsts()
        {
            DeserializeLocalGlobalConsts(GlobalConstsHandler.Instance_,GlobalConstsSerializationPath);
        }

        public static void SerializeLocalGlobalConsts(GlobalConsts target,string path)
        {
            StreamWriter wr;
            if (!File.Exists(path))
            {
                wr = new StreamWriter(File.Create(path));
            }
            else
            {
                wr = new StreamWriter(path, false);
            }
            using (wr)
            {
                wr.WriteLine(JsonUtility.ToJson(target, true));
            }
        }
        public static void DeserializeLocalGlobalConsts(GlobalConsts target,string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), target);
                }
            }
            else
                return;
        }
    }
}