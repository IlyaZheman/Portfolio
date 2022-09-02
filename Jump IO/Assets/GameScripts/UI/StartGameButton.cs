using GameScripts.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    public class StartGameButton : MonoBehaviour
    {
        public Button button;
        
        private GameStarter _gameStarter;

        [Inject]
        public void Construct(GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
            button.OnClickAsObservable().Subscribe(_ => _gameStarter.StartGame());
        }
    }
}