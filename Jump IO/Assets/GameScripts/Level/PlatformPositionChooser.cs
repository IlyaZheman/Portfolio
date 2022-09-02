using System.Collections.Generic;
using GameScripts.Game;
using UniRx;
using UnityEngine;

namespace GameScripts.Level
{
    public class PlatformPositionChooser
    {
        private const int NumberOfLanes = 2;
        private const float LineLenght = 10f;
        private const float LineWidth = 5f;
        private const float MinimalHeight = 0.4f;
        private const float PlayerJumpHeight = 3f;
        private const float PlatformWidth = 1.2f;
        private const float MaxWidth = 5;

        private List<Vector2> _spawnPosition;
        private List<Vector2> _previousPosition;

        private int _platformCount = 20;
        private int _currentPlatform;
        private float _currentPositionY;

        private int _seed;
        private readonly LevelProvider _levelProvider;

        private PlatformPositionChooser(
            LevelProvider levelProvider,
            GameStarter gameStarter)
        {
            _spawnPosition = new List<Vector2>();
            _previousPosition = new List<Vector2>();

            for (var i = 0; i < NumberOfLanes; i++)
            {
                _spawnPosition.Add(new Vector2());
                _previousPosition.Add(new Vector2());
            }

            gameStarter.State.Subscribe(Initialize);
            _levelProvider = levelProvider;
            _levelProvider.CurrentLevel.Subscribe(ChangeLevel);
        }

        // [Inject]
        // private void Constructor(LevelProvider levelProvider,
        //     GameStarter gameStarter)
        // {
        //     _gameStarter = gameStarter;
        //     _gameStarter.State.Subscribe(Initialize);
        //     _levelProvider = levelProvider;
        //     _levelProvider.CurrentLevel.Subscribe(ChangeLevel);
        // }

        private void Initialize(GameState state)
        {
            if (state != GameState.Won && state != GameState.GameOver)
            {
                return;
            }

            _spawnPosition = new List<Vector2>();
            _previousPosition = new List<Vector2>();
            for (var i = 0; i < NumberOfLanes; i++)
            {
                _spawnPosition.Add(new Vector2());
                _previousPosition.Add(new Vector2());
            }

            _currentPlatform = 0;
            _platformCount = 30;
            _currentPositionY = 0;
        }

        private void ChangeLevel(int level)
        {
            _seed = _levelProvider.CurrentLevel.Value;
            Random.InitState(_seed);
        }

        public Vector3 GetPositionNextPlatform()
        {
            var index = FindIndexOfLowerPlatform();
            var border = FindCurrentBorderOnIndex(index);

            var spawnPosition = _spawnPosition[index];
            var previousPosition = _previousPosition[index];

            spawnPosition.x = GetPositionX(border, previousPosition.x);
            spawnPosition.y = GetPositionY(previousPosition.y);

            _spawnPosition[index] = spawnPosition;
            _previousPosition[index] = spawnPosition;

            AddDifficultyLevel();

            return new Vector3(spawnPosition.x, spawnPosition.y, (spawnPosition.y * Mathf.Tan(60)));
        }

        private static float GetPositionX(Vector2 border, float previousPositionX)
        {
            float posX;
            do
            {
                posX = Random.Range(border.x, border.y);
            } while (Mathf.Abs(posX - previousPositionX) < PlatformWidth ||
                     Mathf.Abs(posX - previousPositionX) > MaxWidth);

            return posX;
        }

        private float GetPositionY(float previousPositionY)
        {
            var flag = true;
            float gap;

            do
            {
                gap = Random.Range(0, PlayerJumpHeight);
                var position = _currentPositionY + gap;

                if (position > previousPositionY + MinimalHeight &&
                    position < previousPositionY + PlayerJumpHeight)
                {
                    flag = false;
                }
            } while (flag);

            var nextPosition = _currentPositionY + gap;
            _currentPositionY += LineLenght / _platformCount;

            return nextPosition;
        }

        private void AddDifficultyLevel()
        {
            _currentPlatform++;

            if (_currentPlatform % _platformCount != 0 || _platformCount <= 12)
            {
                return;
            }

            _platformCount -= 2;
        }

        private int FindIndexOfLowerPlatform()
        {
            var currentSpawnPosition = _spawnPosition[0];
            for (var i = 1; i < NumberOfLanes; i++)
            {
                if (currentSpawnPosition.y > _spawnPosition[i].y)
                {
                    currentSpawnPosition = _spawnPosition[i];
                }
            }

            return _spawnPosition.IndexOf(currentSpawnPosition);
        }

        private static Vector2 FindCurrentBorderOnIndex(int indexOfLine)
        {
            const float left = -(LineWidth * ((float)NumberOfLanes / 2));
            var rightBorder = left + LineWidth * (indexOfLine + 1) - (PlatformWidth / 2);
            var leftBorder = rightBorder - LineWidth + (PlatformWidth / 2);

            return new Vector2(leftBorder, rightBorder);
        }
    }
}