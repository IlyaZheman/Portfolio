using GameScripts.Game;
using GameScripts.Level;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.UI.View
{
    public class CurrentLevelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI level;
        private LevelProvider _levelProvider;

        [Inject]
        private void Construct(
            LevelProvider levelProvider,
            GameStarter gameStarter)
        {
            _levelProvider = levelProvider;
            _levelProvider.CurrentLevel.Subscribe(_ => ChangeLevel()).AddTo(this);
        }

        private void ChangeLevel()
        {
            level.text = (_levelProvider.CurrentLevel.Value + 1).ToString();
        }
    }
}