using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Constants;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class FreedTarget : MonoBehaviour, IFixedUpdateHandler
    {
        [Header("Ground Checking")]
        [SerializeField] private float gravityScale = 0.1f;
        [SerializeField] private float checkGroundRadius = 0.3f;
        [SerializeField] private LayerMask groundMask;

        [Space(10)]
        [SerializeField] private Animator animalAnimator;
        [SerializeField] private GameObject shadowObject;

        [Header("Audios")]
        [SerializeField] private AudioClip[] babyClips;
        [SerializeField] private AudioClip[] groundedClips;
        [SerializeField] private AudioSource babySound;

        private static readonly int _groundedHash = Animator.StringToHash("Grounded");

        private float _yVelocity = 0;
        private bool _isGrounded = false;
        
        private Vector3 _movementVector;
        private Vector3 _checkPosition;
        
        private Collider2D _groundCollider;
        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            shadowObject.SetActive(false);
        }

        private void OnEnable()
        {
            PlayFreedAudio();
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public void OnFixedUpdate()
        {
            if (!_isGrounded)
            {
                FallDown();
                CheckGround();
            }
        }

        private void CheckGround()
        {
            _checkPosition = transform.position + new Vector3(0, -0.3f);
            _groundCollider = Physics2D.OverlapCircle(_checkPosition, checkGroundRadius, groundMask);
            
            if(_groundCollider != null)
            {
                _isGrounded = true;
                OnGrounded().Forget();
            }
        }

        private void FallDown()
        {
            _yVelocity += (EntityConstants.Gravity + 0.5f) * gravityScale * Time.fixedDeltaTime;
            _movementVector = new Vector3(0, _yVelocity, 0);
            transform.Translate(_movementVector * Time.fixedDeltaTime);
        }

        private async UniTaskVoid OnGrounded()
        {
            PlayGroundAudio();
            shadowObject.SetActive(true);
            animalAnimator.SetTrigger(_groundedHash);

            await UniTask.Delay(TimeSpan.FromSeconds(0.75f), cancellationToken: _token);
            SimplePool.Despawn(this.gameObject);
        }

        private void PlayFreedAudio()
        {
            int rand = Random.Range(0, babyClips.Length);
            babySound.PlayOneShot(babyClips[rand]);
        }

        private void PlayGroundAudio()
        {
            int rand = Random.Range(0, groundedClips.Length);
            babySound.PlayOneShot(groundedClips[rand]);
        }

        private void OnDisable()
        {
            _isGrounded = false;
            shadowObject.SetActive(false);
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
