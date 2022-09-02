using GameScripts.Characters;
using GameScripts.Game;
using GameScripts.Level;
using GameScripts.Providers.Interface;
using UnityEngine;
using Zenject;

namespace GameScripts.Player
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private ColliderRotate colliderRotate;
        [Space] [SerializeField] private Rigidbody2D playerRigidbody;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform loseZone;
        [SerializeField] private Animator animator;

        [Space] [SerializeField] [Range(0f, 10f)]
        private float rotationSpeed = 5f;

        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float animationSpeed = 1.6f;

        private Platform _platform;
        private bool _isCanMove;
        public bool isNotActive = true;

        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int AnimSpeed = Animator.StringToHash("AnimSpeed");
        private IInputProvider _inputProvider;
        private GameStarter _gameStarter;

        [Inject]
        public void Construct(IInputProvider inputProvider,
            TrackPositionsViewModel trackPositionsViewModel,
            GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
            _inputProvider = inputProvider;
        }

        private void Awake()
        {
            animator.SetFloat(AnimSpeed, animationSpeed);
            loseZone = FindObjectOfType<PlayerLoseZone>().transform;
        }

        // Used by animator event trigger
        public void Jump()
        {
            playerRigidbody.velocity = Vector2.up * jumpForce;
            _isCanMove = true;
            if (_platform != null)
            {
                _platform.TakeHealth();
            }
        }

        private void Update()
        {
            Teleport(); // todo: delete it

            Move();
            CheckingLose();
        }

        private void Teleport()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                transform.position = new Vector3(0, 102f);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (isNotActive)
            {
                return;
            }

            if (playerRigidbody.velocity.y > 0.1f /*|| leg.position.y < col.transform.position.y*/)
            {
                return;
            }

            _isCanMove = false;
            animator.SetTrigger(Jumping);
            _platform = col.gameObject.GetComponent<Platform>();
        }

        private void Move()
        {
            if (!_isCanMove)
            {
                return;
            }

            if (_inputProvider.MoveLeft())
            {
                playerTransform.Translate(Vector2.left * (speed * Time.deltaTime), Space.World);

                var q = Quaternion.AngleAxis(-100, Vector2.up);
                playerTransform.rotation =
                    Quaternion.Slerp(playerTransform.rotation, q, Time.deltaTime * rotationSpeed);
            }

            if (_inputProvider.MoveRight())
            {
                playerTransform.Translate(Vector2.right * (speed * Time.deltaTime), Space.World);

                var q = Quaternion.AngleAxis(100, Vector2.up);
                playerTransform.rotation =
                    Quaternion.Slerp(playerTransform.rotation, q, Time.deltaTime * rotationSpeed);
            }

            colliderRotate.Rotate(playerTransform.position.y);
        }

        private void CheckingLose()
        {
            if (loseZone.position.y < playerTransform.position.y)
            {
                return;
            }

            _gameStarter.GameOver();
            Destroy(gameObject);
        }
    }
}