using GameScripts.Game;
using GameScripts.UIManagement;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI.View
{
    public class GameOverPanelView : MonoBehaviour
    {
        [SerializeField] private UIView gameOverPanel;
        [SerializeField] private PlaceImage imagePlacer;
        [SerializeField] private Button tryAgainButton;
        
        [Inject]
        private void Construct(FinishPanelViewModel viewModel)
        {
            viewModel.State.Subscribe(OnGameOver).AddTo(this);
            tryAgainButton.OnClickAsObservable().Subscribe(_ => viewModel.RebootLevel()).AddTo(this);
        }

        private void OnGameOver(GameState state)
        {
            if (state == GameState.GameOver)
            {
                gameOverPanel.Show();
                imagePlacer.Display(5);
            }
            else
            {
                gameOverPanel.Hide();
            }
        }
    }
}