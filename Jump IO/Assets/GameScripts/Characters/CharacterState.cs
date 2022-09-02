using GameScripts.Bot;
using GameScripts.Game;
using GameScripts.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.Characters
{
    public class CharacterState : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Animator animator;

        private static readonly int DynIdle = Animator.StringToHash("DynIdle");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int WaveDance = Animator.StringToHash("WaveDance");

        private GameStarter _gameStarter;
        private bool _isBot;

        [Inject]
        public void Construct(GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
            gameStarter.State.Subscribe(_ => StartState(_gameStarter.State.Value)).AddTo(this);
        }

        private void StartState(GameState state)
        {
            if (gameObject.GetComponent<BotMove>() != null)
            {
                _isBot = true;
            }

            switch (state)
            {
                case GameState.Ready:
                    SetReady();
                    break;
                case GameState.Playing:
                    SetPlaying();
                    break;
                case GameState.Won:
                    if (!_isBot)
                    {
                        SetWon();
                    }

                    break;
                case GameState.GameOver:
                    SetGameOver();
                    break;
                default:
                    Debug.LogError("Select the correct state!");
                    SetReady();
                    break;
            }
        }

        private void SetReady()
        {
            MoveEnabled(false);

            animator.SetTrigger(DynIdle);
        }

        private void SetPlaying()
        {
            MoveEnabled(true);

            animator.SetTrigger(Jumping);
        }

        private void SetWon()
        {
            MoveEnabled(false);

            animator.SetTrigger(WaveDance);
        }

        private void SetGameOver()
        {
            MoveEnabled(false);
        }

        private void MoveEnabled(bool isEnabled)
        {
            if (player.GetComponent<PlayerMove>() != null)
            {
                var move = player.GetComponent<PlayerMove>();
                move.enabled = isEnabled;
                move.isNotActive = !isEnabled;
            }

            if (player.GetComponent<BotMove>() != null)
            {
                var move = player.GetComponent<BotMove>();
                move.enabled = isEnabled;
                move.isNotActive = !isEnabled;
            }
        }
    }
}