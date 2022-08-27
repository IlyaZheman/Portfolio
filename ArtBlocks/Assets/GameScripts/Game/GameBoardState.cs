using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Game
{
    public class GameBoardState : MonoBehaviour
    {
        [SerializeField] private Transform dragItems;
        [SerializeField] private Hint hint;

        public event Action GameFinished;

        private List<GameItem> _droppedItems;
        private List<GameItem> _notDroppedItems;

        private const int MaxItemInPanel = 3;
        private int _numberOfItemsInPanel = 1;
        private int _allItemsCount;
        private int _currentItemNumber;

        private void Awake()
        {
            _droppedItems = new List<GameItem>();
            _notDroppedItems = new List<GameItem>();
        }

        public void DoVisible()
        {
            if (_numberOfItemsInPanel == 1)
            {
                _numberOfItemsInPanel = MaxItemInPanel;
                for (var i = 0; i < MaxItemInPanel; i++)
                {
                    if (_currentItemNumber == _allItemsCount)
                    {
                        return;
                    }

                    _notDroppedItems[i].gameObject.transform.SetParent(dragItems.GetChild(i));
                    _notDroppedItems[i].gameObject.GetComponent<GameItem>().Parent = dragItems.GetChild(i);

                    var rectParent = dragItems.GetChild(i).transform as RectTransform;
                    var rectItem = _notDroppedItems[i].gameObject.transform as RectTransform;

                    var sizeParent = rectParent!.sizeDelta;
                    var sizeDelta = rectItem!.sizeDelta;

                    rectItem.anchoredPosition = new Vector2(sizeParent.x / 2, -sizeParent.y / 2);

                    float delta;
                    if (rectItem.sizeDelta.x > rectItem.sizeDelta.y)
                    {
                        delta = (sizeDelta.x * 100) / sizeParent.x;
                    }
                    else
                    {
                        delta = (sizeDelta.y * 100) / sizeParent.y;
                    }

                    sizeDelta = sizeDelta / delta * 100;
                    rectItem.sizeDelta = sizeDelta;

                    _notDroppedItems[i].gameObject.SetActive(true);
                    _currentItemNumber++;
                }
            }
            else
            {
                _numberOfItemsInPanel--;
            }
        }

        public void Initialize(List<GameItem> allItems)
        {
            _numberOfItemsInPanel = 1;
            hint.ResetHint();

            foreach (var item in allItems)
            {
                item.Dropped += OnItemDropped;
                _notDroppedItems.Add(item);
            }

            _allItemsCount = _notDroppedItems.Count;
        }

        private void OnItemDropped(GameItem item, bool result)
        {
            if (result)
            {
                _notDroppedItems.Remove(item);
                _droppedItems.Add(item);
                DoVisible();
            }

            Finish();
        }

        private void Finish()
        {
            if (_notDroppedItems.Count != 0)
            {
                return;
            }

            _notDroppedItems.Clear();
            _droppedItems.Clear();
            _currentItemNumber = 0;

            GameFinished?.Invoke();
        }

        public void Unsubscribe()
        {
            foreach (var item in _droppedItems)
            {
                item.Dropped -= OnItemDropped;
            }
        }
    }
}