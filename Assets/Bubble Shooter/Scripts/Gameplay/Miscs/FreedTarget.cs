using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Gameplay.GameManagers;
using BubbleShooter.Scripts.Common.Constants;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class FreedTarget : MonoBehaviour
    {
        [SerializeField] private float moveVelocity = 1.05f;
        [SerializeField] private Animator animalAnimator;
        [SerializeField] private GameObject shadowObject;
        [SerializeField] private GameObject parachute;

        [Header("Audios")]
        [SerializeField] private AudioClip[] babyClips;
        [SerializeField] private AudioClip[] groundedClips;
        [SerializeField] private AudioSource babySound;

        private readonly int _groundedHash = Animator.StringToHash("Grounded");

        private Vector3 _toPosition;
        private Transform _groundPoint;
        private Sequence _moveSequence;

        private void Awake()
        {
            shadowObject.SetActive(false);
        }

        private void Start()
        {
            PlayFreedAudio();
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            _groundPoint = GameController.Instance
                           .GameDecorator.GroundPointContainer;
            _toPosition = GameController.Instance
                          .GameDecorator.GetGroundingPoint();

            transform.SetParent(_groundPoint);
            _toPosition = _groundPoint.parent.InverseTransformPoint(_toPosition);
            _moveSequence ??= CreateMoveTween(_toPosition);
            _moveSequence.Rewind();
            _moveSequence.PlayForward();
            _moveSequence.OnComplete(() =>
            {
                OnGrounded();
            });
        }

        private void OnGrounded()
        {
            PlayGroundAudio();
            shadowObject.SetActive(true);
            parachute.SetActive(false);
            animalAnimator.SetTrigger(_groundedHash);
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

        private Sequence CreateMoveTween(Vector3 toPosition)
        {
            Sequence sequence = DOTween.Sequence();
            float duration = Vector3.Distance(transform.position, toPosition) / moveVelocity;

            sequence.Insert(0, transform.DOLocalMoveX(toPosition.x, duration).SetEase(Ease.Linear));
            sequence.Insert(0, transform.DOLocalMoveY(toPosition.y, duration).SetEase(Ease.InQuad));
            sequence.SetAutoKill(false);

            return sequence;
        }

        private void OnDestroy()
        {
            _moveSequence?.Kill();
        }
    }
}
