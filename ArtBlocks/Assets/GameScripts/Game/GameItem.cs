using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.Game
{
    public class GameItem : MonoBehaviour
    {
        public event Action<GameItem, bool> Dropped;

        private string _itemId;
        private DraggableItem _draggableItem;

        public Transform Parent { get; set; }

        private void Awake()
        {
            _draggableItem = GetComponent<DraggableItem>();
        }

        public void Init()
        {
            Parent = gameObject.transform.parent;
            _itemId = GetComponent<Image>().sprite.name;
        }

        public void OnDrop()
        {
            var hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
            foreach (var hit in hits)
            {
                var hasComponent = hit.transform.TryGetComponent<GameItemSlot>(out var itemSlot);
                if (!hasComponent)
                {
                    continue;
                }

                if (itemSlot.ItemId != _itemId)
                {
                    continue;
                }

                var slot = hit.transform;
                MoveToFinishPosition(slot);
                return;
            }

            MoveToStartPosition();
        }

        private void MoveToStartPosition()
        {
            transform.DOScale(new Vector3(1f, 1f, 0f), 0.07f);
            gameObject.transform.SetParent(Parent);

            var rectItem = gameObject.transform as RectTransform;
            var rectParent = Parent.transform as RectTransform;
            var sizeParent = rectParent.sizeDelta;
            rectItem.anchoredPosition = new Vector2(sizeParent.x / 2, -sizeParent.y / 2);
            rectItem.sizeDelta = _draggableItem.DefaultSize;

            Dropped?.Invoke(this, false);
        }

        private void MoveToFinishPosition(Transform slot)
        {
            transform.DOMove(slot.position, 0.1f);
            gameObject.GetComponent<Image>().raycastTarget = false;
            Dropped?.Invoke(this, true);
        }
    }
}