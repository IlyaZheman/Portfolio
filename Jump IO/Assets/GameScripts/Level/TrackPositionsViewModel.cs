using UniRx;
using UnityEngine;

namespace GameScripts.Level
{
    public class TrackPositionsViewModel
    {
        public readonly IReadOnlyReactiveCollection<Transform> BotPositions;
        public readonly IReactiveProperty<Transform> PlayerPosition;

        private readonly ReactiveCollection<Transform> _botPositions;
        private readonly IReactiveProperty<Transform> _playerPosition;

        public float FinishPosition { get; private set; }
        public float StartPosition { get; private set; }

        public TrackPositionsViewModel()
        {
            _botPositions = new ReactiveCollection<Transform>();
            _playerPosition = new ReactiveProperty<Transform>();
            BotPositions = _botPositions;
            PlayerPosition = _playerPosition;

            FinishPosition = 100;
            StartPosition = 0;
        }

        public void AddBot(Transform transform)
        {
            _botPositions.Add(transform);
        }

        public void RemoveBot(Transform transform)
        {
            _botPositions.Remove(transform);
        }

        public void SetPlayer(Transform transform)
        {
            _playerPosition.Value = transform;
        }

        public void RemovePlayer()
        {
            _playerPosition.Value = null;
        }
    }
}