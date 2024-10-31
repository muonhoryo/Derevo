

using System.IO;
using Derevo.Level;
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
        public const string LevelsSerializationDirectory =
#if UNITY_EDITOR
            "Assets/Scripts/Editor/Levels/";
#else
            "Levels/";
#endif
        public const string LevelsSerializationFormat = ".json";

        public static void SerializeGlobalConsts()
        {
            SerializeLocalGlobalConsts(GlobalConstsHandler.Instance_, GlobalConstsSerializationPath);
        }
        public static void DeserializeGlobalConsts()
        {
            DeserializeLocalGlobalConsts(GlobalConstsHandler.Instance_,GlobalConstsSerializationPath);
        }
        public static void SerializeLevelInfo(LevelManager.LevelMapInfo info,string levelName)
        {
            string path = LevelsSerializationDirectory + levelName + LevelsSerializationFormat;
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
                wr.WriteLine(JsonUtility.ToJson(info));
            }
        }
        public static LevelManager.LevelMapInfo DeserializeMapInfo(string levelName)
        {
            string path=LevelsSerializationDirectory + levelName + LevelsSerializationFormat;
            if (File.Exists(path))
            {
                using(StreamReader sr=new StreamReader(path))
                {
                    return JsonUtility.FromJson<LevelManager.LevelMapInfo>(sr.ReadToEnd());
                }
            }
            else
                return new LevelManager.LevelMapInfo();
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