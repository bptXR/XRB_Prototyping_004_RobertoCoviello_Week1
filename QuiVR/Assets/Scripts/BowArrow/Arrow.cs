using System;
using System.Collections;
using DG.Tweening;
using Enemies;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BowArrow
{
    public class Arrow : XRGrabInteractable
    {
        [SerializeField] private float speed = 2000.0f;
        [SerializeField] private Transform tip;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private MeshRenderer meshRenderer;
        public int damage = 50;
        public int damageToEnemy;

        private bool _isFlying;
        private Vector3 _lastPosition = Vector3.zero;
        private Rigidbody _rigidbody;
        private RaycastHit _hit;
        private PullMeasurer _pullMeasurer;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _pullMeasurer = FindObjectOfType<PullMeasurer>();
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if (args.interactorObject is not Notch notch) return;
            if (notch.CanRelease) LaunchArrow(notch);
        }

        private void LaunchArrow(Notch notch)
        {
            _isFlying = true;
            ApplyForce(notch.PullMeasurer);
            StartCoroutine(LaunchRoutine());
        }

        private void ApplyForce(PullMeasurer pullMeasurer)
        {
            _rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed));
        }

        private IEnumerator LaunchRoutine()
        {
            while (!CheckForCollision(out _hit))
            {
                SetDirection();
                yield return null;
            }

            damageToEnemy = Mathf.RoundToInt(damage * _pullMeasurer.lastPullAmount);

            DisablePhysics();
            ChildArrowToTarget(_hit);
            CheckForHittable(_hit);
            DestroyArrow();
        }

        private bool CheckForCollision(out RaycastHit hit)
        {
            if (_lastPosition == Vector3.zero)
                _lastPosition = tip.position;

            bool collided = Physics.Linecast(_lastPosition, tip.position, out hit, layerMask);
            _lastPosition = collided ? _lastPosition : tip.position;

            return collided;
        }

        private void SetDirection()
        {
            if (_rigidbody.velocity.z > 0.5f)
                transform.forward = _rigidbody.velocity;
        }

        private void DisablePhysics()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        private void DestroyArrow()
        {
            meshRenderer.material.DOFade(0, 6).OnComplete(() => Destroy(gameObject));
        }

        private void ChildArrowToTarget(RaycastHit hit) => transform.SetParent(hit.transform);

        private void CheckForHittable(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out Enemy hittable))
                hittable.Hit(this);
        }

        public override bool IsSelectableBy(IXRSelectInteractor interactor)
        {
            return base.IsSelectableBy(interactor) && !_isFlying;
        }
    }
}