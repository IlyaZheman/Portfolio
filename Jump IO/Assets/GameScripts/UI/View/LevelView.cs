using GameScripts.Game;
using GameScripts.Level;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.UI.View
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI level;
        private LevelProvider _levelProvider;
        private GameStarter _gameStarter;

        [Inject]
        private void Construct(
            LevelProvider levelProvider,
            GameStarter gameStarter)
        {
            _levelProvider = levelProvider;
            _gameStarter = gameStarter;
            _gameStarter.State.Subscribe(ChangeLevel).AddTo(this);
        }

        private void ChangeLevel(GameState state)
        {
            if (state == GameState.Won || state == GameState.GameOver)
            {
                level.text = "LEVEL " + (_levelProvider.CurrentLevel.Value + 1);
            }
        }
    }
}