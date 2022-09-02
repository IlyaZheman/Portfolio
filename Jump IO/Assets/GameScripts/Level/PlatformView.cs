using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.Level
{
    public class PlatformView : MonoBehaviour
    {
        public Platform platform;
        public TextMeshProUGUI health;
        public Image platformImage;
        public Image leftPartImage;
        public Image rightPartImage;
        public GameObject heartImage;

        public Color highHealthColor;
        public Color mediumHealthColor;
        public Color lowHealthColor;
        private Sequence _sequence;

        private void Start()
        {
            platform.Health.Subscribe(UpdateHealth).AddTo(this);
        }

        private void UpdateHealth(int amount)
        {
            platformImage.color = ColorForHealth(amount);
            leftPartImage.color = ColorForHealth(amount);
            rightPartImage.color = ColorForHealth(amount);
            health.text = amount.ToString();
            if (amount == 0)
                SwitchToDestroyedView();
        }

        private Color ColorForHealth(int amount)
        {
            Color newColor;
            if (amount > 3)
            {
                newColor = highHealthColor;
            }
            else if (amount > 1)
            {
                newColor = mediumHealthColor;
            }
            else
            {
                newColor = lowHealthColor;
            }

            return newColor;
        }

        private void SwitchToDestroyedView()
        {
            leftPartImage.gameObject.SetActive(true);
            rightPartImage.gameObject.SetActive(true);
            platformImage.gameObject.SetActive(false);
            heartImage.SetActive(false);
            health.gameObject.SetActive(false);
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, leftPartImage.transform.DOMoveY(-4, 1).SetRelative(true));
            _sequence.Insert(0.0f, rightPartImage.transform.DOMoveY(-4, 1).SetRelative(true));
            _sequence.Insert(0.2f, leftPartImage.DOFade(0, 0.8f));
            _sequence.Insert(0.2f, rightPartImage.DOFade(0, 0.8f));
        }
    }
}