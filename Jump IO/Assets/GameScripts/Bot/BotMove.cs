using System;
using System.Collections.Generic;
using System.Linq;
using GameScripts.Characters;
using GameScripts.Level;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameScripts.Bot
{
    public class BotMove : MonoBehaviour
    {
        [SerializeField] private ColliderRotate colliderRotate;
        [SerializeField] private CheckZone checkZone;
        [Space] [SerializeField] private Rigidbody2D botRigidbody;
        [SerializeField] private Transform botTransform;
        [SerializeField] private Animator animator;

        [Space] [SerializeField] [Range(0f, 10f)]
        private float rotationSpeed = 5f;

        [SerializeField] private float animationSpeed = 1.6f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float speed = 300f;
        [Space] [SerializeField] private List<Transform> platforms;

        private Platform _platform;
        private Vector3 _platformIStand;
        private Vector3 _nextPlatformPosition;
        private float _positionY;
        private bool _isCanMove;
        private bool _isChecked;
        public bool isNotActive = true;
        private int _randomPlatform;

        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int AnimSpeed = Animator.StringToHash("AnimSpeed");
        private TrackPositionsViewModel _trackPositionsViewModel;

        [Inject]
        private void Construct(TrackPositionsViewModel trackPositionsViewModel)
        {
            _trackPositionsViewModel = trackPositionsViewModel;
        }

        private void Awake()
        {
            animator.SetFloat(AnimSpeed, animationSpeed);
        }

        // Used by animator event trigger
        public void Jump()
        {
            botRigidbody.velocity = Vector2.up * jumpForce;
            _isCanMove = true;
            if (_platform != null)
            {
                _platform.TakeHealth();
            }
        }

        private void Update()
        {
            CheckingNextPlatform();

            if (Math.Abs(_nextPlatformPosition.x - botTransform.position.x) > 0.1f)
            {
                Move(_nextPlatformPosition);
            }

            CheckingLose();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (isNotActive || (botRigidbody.velocity.y > 0.1f /*|| leg.position.y < col.transform.position.y*/))
            {
                return;
            }

            _positionY = botTransform.position.y;
            _isCanMove = false;

            platforms = new List<Transform>(checkZone.PlatformsInTrigger);

            _platform = col.gameObject.GetComponent<Platform>();
            _platformIStand = _platform.transform.position;

            SortingPlatforms();
            _isChecked = true;

            animator.SetTrigger(Jumping);
        }

        private void CheckingNextPlatform()
        {
            if (!_isCanMove)
            {
                return;
            }

            CheckingBrokenPlatforms();

            if (platforms.Count == 0)
            {
                return;
            }

            ChoosingPlatformPosition();
        }

        private void CheckingBrokenPlatforms()
        {
            var platformsCopy = new List<Transform>(platforms);

            foreach (var platform in platformsCopy)
            {
                if (platform == null || platform.position.y < _platformIStand.y)
                {
                    platforms.Remove(platform);
                }
            }
        }

        private void ChoosingPlatformPosition()
        {
            if (_isChecked)
            {
                _randomPlatform = Random.Range(0, platforms.Count - 1);
                _isChecked = false;
            }
            else if (_randomPlatform > platforms.Count - 1)
            {
                _randomPlatform = platforms.Count - 1;
            }

            _nextPlatformPosition = platforms[_randomPlatform].position;
        }

        private void Move(Vector3 platformPosition)
        {
            if (!_isCanMove)
            {
                return;
            }

            if (platformPosition.x < botTransform.position.x)
            {
                botTransform.Translate(Vector2.left * (speed * Time.deltaTime), Space.World);

                var q = Quaternion.AngleAxis(-100, Vector2.up);
                botTransform.rotation = Quaternion.Slerp(botTransform.rotation, q, Time.deltaTime * rotationSpeed);
            }

            if (platformPosition.x > botTransform.position.x)
            {
                botTransform.Translate(Vector2.right * (speed * Time.deltaTime), Space.World);

                var q = Quaternion.AngleAxis(100, Vector2.up);
                botTransform.rotation = Quaternion.Slerp(botTransform.rotation, q, Time.deltaTime * rotationSpeed);
            }

            colliderRotate.Rotate(botTransform.rotation.y);
        }

        private void CheckingLose()
        {
            if (_positionY - botTransform.position.y < 4)
            {
                return;
            }

            _trackPositionsViewModel.RemoveBot(transform);
            Destroy(gameObject);
        }

        private void SortingPlatforms()
        {
            var platformIStand = platforms.FirstOrDefault(o => o.transform.position == _platformIStand);
            platforms.Remove(platformIStand);

            if (platforms.Count == 0)
            {
                return;
            }

            for (var i = 0; i < platforms.Count; i++)
            {
                for (var j = 1; j < platforms.Count; j++)
                {
                    if (platforms[i].transform.position.y < platforms[j].transform.position.y)
                    {
                        (platforms[i], platforms[j]) = (platforms[j], platforms[i]);
                    }
                }
            }
        }
    }
}