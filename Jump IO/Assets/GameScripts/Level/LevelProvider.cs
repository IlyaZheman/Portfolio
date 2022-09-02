using UniRx;
using UnityEngine;

namespace GameScripts.Level
{
    public class LevelProvider
    {
        private const string Key = "currentLevel";

        public readonly ReactiveProperty<int> CurrentLevel;

        public LevelProvider()
        {
            CurrentLevel = new ReactiveProperty<int>(0);
            CurrentLevel.Value = PlayerPrefs.GetInt(Key, 0);
            CurrentLevel.Subscribe(level => PlayerPrefs.SetInt(Key, level));
        }
    }
}