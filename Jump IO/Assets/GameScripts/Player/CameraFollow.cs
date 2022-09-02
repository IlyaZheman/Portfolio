using GameScripts.Game;
using GameScripts.Level;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 1;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector2 horizontalBorder;

        private Vector3 _defaultPosition;
        private GameStarter _gameStarter;

        [Inject]
        private void Construct(GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
            _defaultPosition = transform.position;
            _gameStarter.State.Subscribe(ChangeTarget).AddTo(this);
        }

        private void ChangeTarget(GameState state)
        {
            switch (state)
            {
                case GameState.Ready:
                    target = null;
                    transform.position = _defaultPosition;
                    break;
                case GameState.Playing:
                    target = FindObjectOfType<PlayerSpawnPoint>().transform.GetChild(0);
                    break;
            }
        }

        private void Update()
        {
            if (!target)
            {
                return;
            }

            var position = transform.position;
            var targetPosition = target.position;

            var posX = Mathf.Lerp(position.x, targetPosition.x, speed * Time.deltaTime);
            var posY = position.y;
            var posZ = Mathf.Lerp(position.z, targetPosition.z - offset.z, speed * Time.deltaTime);
            posX = Mathf.Clamp(posX, horizontalBorder.x, horizontalBorder.y);

            if (target.position.y - offset.y > transform.position.y)
            {
                posY = Mathf.Lerp(transform.position.y, targetPosition.y - offset.y, speed * Time.deltaTime);
            }

            transform.position = new Vector3(posX, posY, posZ);
        }
    }
}