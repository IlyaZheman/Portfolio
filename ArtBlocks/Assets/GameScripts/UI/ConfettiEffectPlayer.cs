using GameScripts.Game;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    public class ConfettiEffectPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem system;

        [Inject]
        public void Construct(GameBoardState gameBoardState)
        {
            gameBoardState.GameFinished += system.Play;
        }
    }
}