using System.Collections;
using GameScripts.Game;
using UnityEngine;
using Zenject;

namespace GameScripts.Level
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] private GameObject confettiPrefab;

        private GameStarter _gameStarter;
        private LevelProvider _levelProvider;

        [Inject]
        private void Constructor(GameStarter gameStarter,
            LevelProvider levelProvider)
        {
            _levelProvider = levelProvider;
            _gameStarter = gameStarter;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player"))
            {
                return;
            }

            _gameStarter.WonGame();
            _levelProvider.CurrentLevel.Value++;

            var playerPosition = col.gameObject.transform.position;
            StartCoroutine(InstantiateConfetti(playerPosition));
        }

        private IEnumerator InstantiateConfetti(Vector3 playerPosition)
        {
            yield return new WaitForSeconds(0.5f);
            var confettiRight = Instantiate(confettiPrefab, new Vector3(playerPosition.x + 3f, 103f, playerPosition.z),
                Quaternion.identity, transform);
            var confettiLeft = Instantiate(confettiPrefab, new Vector3(playerPosition.x - 3f, 103f, playerPosition.z),
                Quaternion.identity, transform);
        }
    }
}