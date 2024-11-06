

using System;
using Derevo.PlayerControl;
using Derevo.Serialization;
using UnityEngine;

namespace Derevo.Level
{
    public sealed class GameSceneLevelInitialization : MonoBehaviour
    {
        public static string LevelName;
        public static event Action LevelLoadingDoneEvent = delegate { };
        private void Start()
        {
            LevelManager.LevelMapInfo info = LevelMapSerialization.DeserializeMapInfo(LevelName);
            LevelManager.InitializeLevel(info);
            PlayerControlLocker.Unlock();
            LevelLoadingDoneEvent();
        }
    }
}