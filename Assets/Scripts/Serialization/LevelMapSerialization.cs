

using Derevo.Level;
using System.IO;
using UnityEngine;

namespace Derevo.Serialization
{
    public static class LevelMapSerialization
    {
        public const string LevelsSerializationDirectory =
#if UNITY_EDITOR
            "Assets/Scripts/Editor/Levels/";
#else
            "Levels/";
#endif
        public const string LevelsSerializationFormat = ".json";

        public static void SerializeLevelInfo(LevelManager.LevelMapInfo info, string levelName)
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
                wr.WriteLine(JsonUtility.ToJson(info,true));
            }
        }
        public static LevelManager.LevelMapInfo DeserializeMapInfo(string levelName)
        {
            string path = LevelsSerializationDirectory + levelName + LevelsSerializationFormat;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return JsonUtility.FromJson<LevelManager.LevelMapInfo>(sr.ReadToEnd());
                }
            }
            else
                return new LevelManager.LevelMapInfo();
        }
    }
}