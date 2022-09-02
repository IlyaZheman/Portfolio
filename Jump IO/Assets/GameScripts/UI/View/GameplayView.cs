using GameScripts.Game;
using GameScripts.UIManagement;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.UI.View
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private UIView gameplay;
        
        private GameStarter _gameStarter;

        [Inject]
        private void Construct(GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
            _gameStarter.State.SkipLatestValueOnSubscribe().Subscribe(ShowButton).AddTo(this);
        }

        private void ShowButton(GameState state)
        {
            if (state == GameState.Playing)
            {
                gameplay.Show();
            }
            else
            {
                gameplay.Hide();
            }
        }
    }
}