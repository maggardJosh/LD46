using System.Collections.Generic;
using Entities.Player.States;
using Entity.Base;
using UnityEngine;

namespace Entities.Player
{
    [RequireComponent(typeof(BaseEntity))]
    public class ShovelGrappleHook : MonoBehaviour
    {
        public float speed = 5f;

        public float length = 5f;

        private Vector3 _startPos;
        private Vector3 _offset;

        public delegate void ReturnHook(bool hitSomething);

        public event ReturnHook OnReturnHook;

        void Awake()
        {
            _baseEntity = GetComponent<BaseEntity>();
            _startPos = transform.position;
        }

        private BaseEntity _baseEntity;
        private PlayerController _controller;
        public SpriteRenderer vine;

        private bool _hasBeenActivated = false;
        private Vector3 _direction;

        private void ShootGrapple(PlayerController controller, Vector3 direction)
        {
            _direction = direction;
            _controller = controller;
            _offset = transform.position - controller.transform.position;

            if (CollidesWithWallInstantly())
            {
                OnReturnHook?.Invoke(false);
                return;
            }

            _hasBeenActivated = true;
        }

        private bool CollidesWithWallInstantly()
        {
            var result = _baseEntity.GetMoveTester()
                .GetMoveResult(transform.position - transform.rotation * _direction * 1f, transform.rotation * _direction * 1f);
            foreach (var hit in result.HorizontalHits)
                hit.collider.GetComponent<IDamageable>()?.TakeDamage(_controller);
            foreach (var hit in result.VerticalHits)
                hit.collider.GetComponent<IDamageable>()?.TakeDamage(_controller);
            return result.HitAny;
        }

        public void ShootGrapple(PlayerController controller)
        {
            ShootGrapple(controller, Vector3.right);
        }

        public void ShootGrappleUp(PlayerController controller)
        {
            ShootGrapple(controller, Vector3.up);
        }

        void Update()
        {
            Vector3 diff = transform.position - (_controller.transform.position + _offset);
            var s = vine.size;
            s.x = diff.magnitude;
            vine.size = s;
            _baseEntity.SetVelocity(Vector3.zero);
            if (!_hasBeenActivated)
                return;

            _baseEntity.SetVelocity(transform.rotation * _direction * speed);

            if (_baseEntity.LastHitResult.HitAny)
            {
                IDamageable damageableEnemy = GetDamageableEnemy(_baseEntity.LastHitResult.HorizontalHits,
                    _baseEntity.LastHitResult.VerticalHits);
                if (damageableEnemy != null)
                {
                    damageableEnemy.TakeDamage(_controller);
                    OnReturnHook?.Invoke(false);
                }
                else
                    OnReturnHook?.Invoke(true);

                _hasBeenActivated = false;
                return;
            }

            var dist = (_baseEntity.transform.position - _startPos).sqrMagnitude;
            if (dist > length * length)
            {
                OnReturnHook?.Invoke(false);
                _baseEntity.SetVelocity(Vector3.zero);
                _hasBeenActivated = false;
            }
        }

        private IDamageable GetDamageableEnemy(List<RaycastHit2D> horizontalHits, List<RaycastHit2D> verticalHits)
        {
            IDamageable damageableEnemy = null;
            foreach (var hit in horizontalHits)
            {
                damageableEnemy = hit.collider.GetComponent<IDamageable>();
                if (damageableEnemy != null)
                    return damageableEnemy;
            }

            foreach (var hit in verticalHits)
            {
                damageableEnemy = hit.collider.GetComponent<IDamageable>();
                if (damageableEnemy != null)
                    return damageableEnemy;
            }

            return null;
        }
    }
}