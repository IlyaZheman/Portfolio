using GameScripts.Game;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    public class LevelNameView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        private GameBootstrapper _bootstrapper;

        [Inject]
        public void Construct(GameBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        private void Start()
        {
            _bootstrapper.CurrentLevel.Subscribe(level => levelText.text = $"Level {level + 1}").AddTo(this);
        }
    }
}