using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GameScripts.Game
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private ItemsFactory itemsFactory;
        [SerializeField] private GameBoardState gameBoardState;
        [SerializeField] private WinScreenView winScreenView;
        [SerializeField] private List<GameObject> levels;
        [SerializeField] private Transform outlineParent;

        public IReactiveProperty<int> CurrentLevel;

        private void Awake()
        {
            CurrentLevel = new ReactiveProperty<int>(0);
            StartGame(CurrentLevel.Value);
        }

        private void OnEnable()
        {
            gameBoardState.GameFinished += FinishGame;
        }


        private void StartGame(int levelIndex)
        {
            Instantiate(levels[levelIndex], outlineParent);
            itemsFactory.CreateItems();
            gameBoardState.Initialize(itemsFactory.allItems);
            gameBoardState.DoVisible();
            winScreenView.WinScreenDisable();
        }

        private void FinishGame()
        {
            winScreenView.WinScreenEnable();
            gameBoardState.Unsubscribe();
            ClearLevel();
        }

        private void ClearLevel()
        {
            for (var child = 0; child < outlineParent.childCount; child++)
            {
                Destroy(outlineParent.GetChild(child).gameObject);
            }
        }

        public void NextLevelClick()
        {
            itemsFactory.DestroyItems();
            IncreaseCurrentLevel();
            StartGame(CurrentLevel.Value);
        }

        private void IncreaseCurrentLevel()
        {
            CurrentLevel.Value++;
            if (CurrentLevel.Value == levels.Count)
            {
                CurrentLevel.Value = 0;
            }
        }

        public void ReplayClick()
        {
            itemsFactory.DestroyItems();
            StartGame(CurrentLevel.Value);
        }
    }
}