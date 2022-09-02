using GameScripts.Game;
using GameScripts.UI.View;
using UniRx;

namespace GameScripts.UI
{
    public class FinishPanelViewModel
    {
        public readonly IReadOnlyReactiveProperty<GameState> State;

        private readonly GameStarter _gameStarter;
        private readonly TrackPositionsView _trackPositionsView;

        public int PlayerPlace => _trackPositionsView.GetPlayerPlace();

        public FinishPanelViewModel(GameStarter gameStarter, TrackPositionsView trackPositionsView)
        {
            _trackPositionsView = trackPositionsView;
            _gameStarter = gameStarter;
            State = gameStarter.State;
        }

        public void RebootLevel()
        {
            _gameStarter.PreloadLevel();
        }
        
        public void LoadNextLevel()
        {
            _gameStarter.PreloadLevel();
        }
    }
}