using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameScripts.Game
{
    public class DraggableItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private GameItem _gameItem;

        public Transform DragHandlerContainer { get; set; }
        public Vector2 DefaultSize { get; private set; }

        private void Start()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _canvas = gameObject.transform.parent.parent.GetComponent<Canvas>();
            _gameItem = gameObject.GetComponent<GameItem>();
            DefaultSize = _rectTransform.sizeDelta;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            gameObject.GetComponent<Image>().SetNativeSize();
            transform.DOScale(new Vector3(3f, 3f, 0f), 0.2f);
            _rectTransform.DOAnchorPosY(230f, 0.1f).OnComplete(() =>
            {
                gameObject.transform.SetParent(DragHandlerContainer, true);
            });
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.DOKill();
            gameObject.transform.SetParent(DragHandlerContainer, true);
            transform.DOScale(new Vector3(3f, 3f, 0f), 0.2f);

            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOKill();
            _gameItem.OnDrop();
        }
    }
}