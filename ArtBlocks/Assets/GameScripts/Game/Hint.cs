using System.Collections;
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.Game
{
    public class Hint : MonoBehaviour
    {
        [SerializeField] private Transform gameItems;
        [SerializeField] private GameObject outline;
        [SerializeField] private Transform hintCanvas;
        [SerializeField] [Range(0f, 2f)] private float animationTime = 0.7f;

        private GameObject _item;
        private GameObject _slot;
        private bool _isActive = true;

        public void ResetHint() => _isActive = true;

        private void FindActiveItem()
        {
            for (var i = 0; i < gameItems.childCount; i++)
            {
                var item = gameItems.GetChild(i);
                if (item.childCount == 0)
                {
                    continue;
                }

                item = item.GetChild(0);
                if (!item.gameObject.activeSelf)
                {
                    continue;
                }

                _item = item.gameObject;
                return;
            }
        }

        private void FindSlot()
        {
            var spriteName = _item.gameObject.GetComponent<Image>().sprite.name;
            var images = outline.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                if (spriteName == image.sprite.name)
                {
                    _slot = image.gameObject;
                }
            }
        }

        public void GetHint()
        {
            if (!_isActive)
            {
                return;
            }

            FindActiveItem();
            FindSlot();
            StartCoroutine(SmoothColor(animationTime));
            _isActive = false;
        }

        IEnumerator SmoothColor(float time)
        {
            var hintSlot = Instantiate(_slot, _slot.transform.position, Quaternion.identity, hintCanvas);
            hintSlot.GetComponent<UIEffect>().colorFactor = 0f;
            var alfa = hintSlot.AddComponent<CanvasGroup>();

            for (var i = 0; i < 3; i++)
            {
                var currTime = 0f;
                do
                {
                    alfa.alpha = Mathf.Lerp(0, 1, currTime / time);
                    currTime += Time.deltaTime;
                    yield return null;
                } while (currTime <= (time / 2));

                do
                {
                    alfa.alpha = Mathf.Lerp(1, 0, currTime / time);
                    currTime += Time.deltaTime;
                    yield return null;
                } while (currTime <= time);
            }

            Destroy(hintSlot);
            yield break;
        }
    }
}