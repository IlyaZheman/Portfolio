using System;
using GameScripts.Providers.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.Providers.Module
{
    public class SwipeInputProvider : MonoBehaviour, IInputProvider, IDragHandler, IBeginDragHandler,IEndDragHandler
    {
        private const float Threshold = 10f;
        private float _dragDelta;

        public bool MoveLeft()
        {
            return _dragDelta < -Threshold;
        }

        public bool MoveRight()
        {
            return _dragDelta > Threshold;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Math.Sign(_dragDelta) != Math.Sign(eventData.delta.x))
            {
                _dragDelta = 0;
            }

            _dragDelta += eventData.delta.x;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragDelta = 0;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _dragDelta = 0;
        }
    }
}