using GameScripts.Game;
using GameScripts.Misc;
using GameScripts.Providers.Module;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.Level
{
    public class PlatformSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject defaultPlatform;
        [SerializeField] private Platform platformPrefab;
        [SerializeField] private Transform spawnContainer;

        private PlatformPositionChooser _platformPositionChooser;
        private GameStarter _gameStarter;

        private Vector3 _position;
        private FinishFactory _finishFactory;
        private LevelConfiguration _levelConfiguration;

        [Inject]
        private void Constructor(
            PlatformPositionChooser platformPositionChooser,
            GameStarter gameStarter,
            FinishFactory finishFactory,
            LevelConfiguration levelConfiguration)
        {
            _finishFactory = finishFactory;
            _platformPositionChooser = platformPositionChooser;
            _levelConfiguration = levelConfiguration;
            _gameStarter = gameStarter;
            _gameStarter.State
                .Where(s => s == GameState.Ready)
                .Subscribe(_ => SpawnPlatforms(_levelConfiguration.levelHeight))
                .AddTo(this);
        }

        private void SpawnPlatforms(float upperBound)
        {
            _position = new Vector3();
            spawnContainer.DestroyAllChildren();

            while (_position.y < upperBound)
            {
                SpawnPlatform(spawnContainer, upperBound);
            }

            var finishPositionY = _position.y + 1.2f;
            var finish = _finishFactory.Create();
            finish.transform.position = new Vector3(0f, finishPositionY, finishPositionY * Mathf.Tan(60));
            finish.transform.SetParent(spawnContainer);

            Instantiate(defaultPlatform, new Vector3(), Quaternion.identity, spawnContainer);
        }

        private void SpawnPlatform(Transform container, float finishPositionY)
        {
            if (finishPositionY < _position.y)
            {
                return;
            }

            var platformHealthGenerator =
                new PlatformHealthGenerator(_levelConfiguration.weightsPresets, _levelConfiguration.hpAmount);
            var platformHealth = platformHealthGenerator.GenerateHealth(_position.y);
            _position = _platformPositionChooser.GetPositionNextPlatform();

            var platform = Instantiate(platformPrefab, _position, Quaternion.identity, container);
            platform.Health.Value = platformHealth;
        }
    }
}