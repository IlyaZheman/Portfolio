using System.Collections;
using GameScripts.Game;
using GameScripts.UIManagement;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI.View
{
    public class WinPanelView : MonoBehaviour
    {
        [SerializeField] private UIView winPanel;
        [SerializeField] private PlaceImage imagePlacer;
        [SerializeField] private Button continueButton;
        [SerializeField] private float displayDilay = 2f;
        
        private FinishPanelViewModel _viewModel;

        [Inject]
        private void Construct(FinishPanelViewModel viewModel)
        {
            _viewModel = viewModel;
            viewModel.State.Subscribe(OnWin).AddTo(this);
            continueButton.OnClickAsObservable().Subscribe(_ => viewModel.LoadNextLevel()).AddTo(this);
        }

        private void OnWin(GameState state)
        {
            if (state == GameState.Won)
            {
                StartCoroutine(DilayPanelShow());
            }
            else
            {
                winPanel.Hide();
            }
        }

        private IEnumerator DilayPanelShow()
        {
            yield return new WaitForSeconds(displayDilay);
            winPanel.Show();
            imagePlacer.Display(_viewModel.PlayerPlace);
        }
    }
}