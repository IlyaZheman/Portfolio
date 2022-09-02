using System.Collections.Generic;
using System.Linq;
using GameScripts.Characters;
using GameScripts.Game;
using GameScripts.Level;
using GameScripts.Misc;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.Player
{
    public class PlayerSpawner : MonoBehaviour
    {
        private List<PlayerSpawnPoint> _playerSpawn;

        private PlayerStateFactory _playerStateFactory;
        private GameStarter _gameStarter;
        private TrackPositionsViewModel _trackPositionsViewModel;

        [Inject]
        private void Constructor(
            GameStarter gameStarter,
            PlayerStateFactory playerStateFactory,
            TrackPositionsViewModel trackPositionsViewModel)
        {
            _playerStateFactory = playerStateFactory;
            _trackPositionsViewModel = trackPositionsViewModel;
            _gameStarter = gameStarter;
            _gameStarter.State
                .Where(s => s == GameState.Ready)
                .Subscribe(_ => Spawn())
                .AddTo(this);
        }

        private void Spawn()
        {
            _playerSpawn = FindObjectsOfType<PlayerSpawnPoint>().ToList();
            DestroyPlayer(_playerSpawn);

            foreach (var point in _playerSpawn)
            {
                var pointTransform = point.transform;
                var player = _playerStateFactory.Create();

                var playerTransform = player.transform;
                playerTransform.position = pointTransform.position;
                playerTransform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                playerTransform.SetParent(pointTransform);

                var offset = player.GetComponent<CharacterOffset>();
                offset.ChangeOffset(5);

                _trackPositionsViewModel.SetPlayer(playerTransform);
            }
        }

        private void DestroyPlayer(List<PlayerSpawnPoint> spawns)
        {
            foreach (var spawnPoint in spawns)
            {
                var point = spawnPoint.transform;
                if (point.childCount == 0)
                {
                    continue;
                }

                _trackPositionsViewModel.RemovePlayer();
                point.DestroyAllChildren();
            }
        }
    }
}