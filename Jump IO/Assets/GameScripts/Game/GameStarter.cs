using System;
using UniRx;

namespace GameScripts.Game
{
    public class GameStarter
    {
        public readonly IReadOnlyReactiveProperty<GameState> State;

        private readonly IReactiveProperty<GameState> _state;

        private GameStarter()
        {
            _state = new ReactiveProperty<GameState>(GameState.Ready);
            State = _state;
        }

        public void StartGame()
        {
            if (_state.Value != GameState.Ready)
                throw new InvalidOperationException("Invalid state transition");
            _state.Value = GameState.Playing;
        }

        public void WonGame()
        {
            if (_state.Value != GameState.Playing)
                throw new InvalidOperationException("Invalid state transition");
            _state.Value = GameState.Won;
        }

        public void GameOver()
        {
            if (_state.Value != GameState.Playing)
                throw new InvalidOperationException("Invalid state transition");
            _state.Value = GameState.GameOver;
        }

        public void PreloadLevel()
        {
            if (_state.Value != GameState.Won && _state.Value != GameState.GameOver)
                throw new InvalidOperationException("Invalid state transition");

            _state.Value = GameState.Ready;
        }
    }
}