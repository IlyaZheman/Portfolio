using System.Collections.Generic;
using System.Linq;
using GameScripts.Game;
using GameScripts.Level;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI.View
{
    public class TrackPositionsView : MonoBehaviour
    {
        public Slider botMarkPrefab;
        public Slider playerMarkPrefab;
        public Transform container;
        
        [SerializeField] private List<Material> colors;

        private TrackPositionsViewModel _viewModel;
        private readonly Dictionary<Transform, Slider> _bots = new Dictionary<Transform, Slider>();
        private Transform _playerTransform;
        private Slider _playerSlider;
        private GameStarter _gameStarter;
        private bool _onPlaying;
        private int _index;
        private int Index
        {
            get => _index;
            set
            {
                _index = value;
                
                if (_index >= colors.Count)
                {
                    _index %= colors.Count;
                }
            }
        }

        [Inject]
        public void Construct(TrackPositionsViewModel viewModel,
            GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
            _gameStarter.State.Subscribe(OnPlay).AddTo(this);
            _viewModel = viewModel;
            viewModel.BotPositions.ObserveAdd().Subscribe(OnAddBot).AddTo(this);
            viewModel.BotPositions.ObserveRemove().Subscribe(OnRemoveBot);
            viewModel.BotPositions.ObserveReset().Subscribe(_ => OnClearAllBots()).AddTo(this);
            
            viewModel.PlayerPosition.Subscribe(OnPlayerChanged).AddTo(this);
        }

        private void OnPlay(GameState state)
        {
            _onPlaying = state == GameState.Playing;
        }
        
        private void Update()
        {
            if (_playerTransform == null)
                return;
            
            if (!_onPlaying || _viewModel.PlayerPosition.Value == null)
            {
                return;
            }
            
            _playerSlider.value = RemapBetween(_playerTransform.position.y, 
                _viewModel.StartPosition, _viewModel.FinishPosition);
            
            foreach (var bot in _bots)
            {
                bot.Value.value = RemapBetween(bot.Key.transform.position.y, 
                    _viewModel.StartPosition, _viewModel.FinishPosition);
            }
        }

        public int GetPlayerPlace()
        {
            var place = 0;
            
            foreach (var botTransform in _bots.Keys.OrderBy(_ => _.transform.position.y))
            {
                place++;
            
                if (_playerTransform.position.y <= botTransform.position.y)
                {
                    return (_bots.Count + 1) - place;
                }
            }
            
            return _bots.Count - place + 1;
        }

        private static float RemapBetween(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        private void OnAddBot(CollectionAddEvent<Transform> addEvent)
        {
            var slider = CreateNewBotSlider();
            slider.GetComponentInChildren<BotSliderColorChanger>().ChangeColor(colors[Index].color);
            _bots.Add(addEvent.Value, slider);
            Index++;
        }

        private void OnRemoveBot(CollectionRemoveEvent<Transform> removeEvent)
        {
            Destroy(_bots[removeEvent.Value].gameObject);
            _bots.Remove(removeEvent.Value);
        }

        private void OnClearAllBots()
        {
            foreach (var slider in _bots.Values)
            {
                Destroy(slider.gameObject);
            }
            _bots.Clear();
        }

        private void OnPlayerChanged(Transform player)
        {
            if (_playerSlider != null)
                Destroy(_playerSlider.gameObject);
            
            if (player == null)
            {
                _playerTransform = null;
                return;
            }
        
            _playerTransform = player;
            _playerSlider = CreateNewPlayerSlider();
        }

        private Slider CreateNewBotSlider()
        {
            return Instantiate(botMarkPrefab, container);
        }
        
        private Slider CreateNewPlayerSlider()
        {
            return Instantiate(playerMarkPrefab, container);
        }
    }
}