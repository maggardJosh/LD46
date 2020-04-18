using System;
using UnityEngine;

namespace Entity.Base
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(ForceZAxis))]
    public sealed class BaseEntity : MonoBehaviour
    {
        public bool debugEntity = false;

        public EntitySettings settings;

        public override string ToString()
        {
            return $"pos: {transform.position}, hit: {LastHitResult}, v: {_velocity}";
        }

        public MoveResult LastHitResult { get; private set; } = new MoveResult();
        private Vector3 _velocity = Vector3.zero;
        [NonSerialized] public Vector3 LastVelocity = Vector3.zero;
        [NonSerialized] public BoxCollider2D BoxCollider;

        private Animator _animController;

        private void Awake()
        {
            BoxCollider = GetComponent<BoxCollider2D>();
            _animController = GetComponent<Animator>();
            BaseEntityManager.Instance.AddEntity(this);
        }

        private void OnDestroy()
        {
            BaseEntityManager.Instance.RemoveEntity(this);
        }

        private void FixedUpdate()
        {
            if (_ignoreOneWayTimeLeft > 0)
                _ignoreOneWayTimeLeft -= Time.fixedDeltaTime;
            HandleFixedUpdate();
        }

        public void SetXVelocity(float xVel)
        {
            _velocity.x = xVel;
            ClampVelocity();
        }

        public void SetYVelocity(float yVel)
        {
            _velocity.y = yVel;
            ClampVelocity();
        }

        public float GetXVelocity()
        {
            return _velocity.x;
        }

        public float GetYVelocity()
        {
            return _velocity.y;
        }

        public void AddToVelocity(Vector3 vel)
        {
            _velocity += vel;
            ClampVelocity();
        }

        private void ClampVelocity()
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -settings.MaxXVel, settings.MaxXVel);
            _velocity.y = Mathf.Clamp(_velocity.y, settings.MinYVel, settings.MaxYVel);
        }

        private void HandleFixedUpdate()
        {
            LastVelocity = _velocity;
            TryMove();
            HandleFriction();
            if (settings.ObeysGravity)
                AddToVelocity(Vector3.down * (GameSettings.Gravity * settings.GravityMultiplier * Time.fixedDeltaTime * (1 / .02f)));
        }

        private void TryMove()
        {
            ForceOutOfWalls();

            LastHitResult = GetMoveTester().GetMoveResult(transform.position, _velocity * Time.fixedDeltaTime);
            transform.position = LastHitResult.NewPos;

            if (LastHitResult.HitDown)
                _velocity.y *= settings.YBounceValue;

            if (LastHitResult.HitLeft || LastHitResult.HitRight)
                _velocity.x *= settings.XBounceValue;
        }

        public void TryTransformTo(Vector3 pos)
        {
            ForceOutOfWalls();

            LastHitResult = GetMoveTester().GetMoveResult(transform.position, pos - transform.position);
            transform.position = LastHitResult.NewPos;
        }

        public EntityMoveTester GetMoveTester()
        {
            return GetMoveTester(settings.CollideMask, _ignoreOneWayTimeLeft > 0 ? new LayerMask() : settings.OneWayMask);
        }

        public EntityMoveTester GetMoveTester(LayerMask collisionLayerMask, LayerMask oneWayLayerMask)
        {
            return new EntityMoveTester(new EntityDebugLogger(this), BoxCollider, collisionLayerMask, oneWayLayerMask);
        }

        public void JumpDownThroughOneWay()
        {
            _ignoreOneWayTimeLeft = .1f;
        }

        private float _ignoreOneWayTimeLeft = 0;

        public void ForceOutOfWalls()
        {
            Vector3 position = transform.position;
            MoveResult hitResult = GetMoveTester().GetVerticalRaycastFromCenter(position);
            position = hitResult.NewPos;

            hitResult = GetMoveTester().GetHorizontalRaycastFromCenter(position);
            position = hitResult.NewPos;
            transform.position = position;
        }

        private void HandleFriction()
        {
            if (LastHitResult.HitDown)
                _velocity.x *= settings.GroundFriction;
            else
                _velocity.x *= settings.Friction;

            if (!settings.ObeysGravity)
                _velocity.y *= settings.Friction;
            else
            {
                if (LastHitResult.HitUp && _velocity.y > 0)
                    _velocity.y = 0;
            }
        }

        public Vector3 GetVelocity()
        {
            return _velocity;
        }

        public Vector3 GetTopCenterOfBoxCollider()
        {
            return transform.position +
                   new Vector3(BoxCollider.offset.x, BoxCollider.offset.y + BoxCollider.size.y / 2f);
        }

        public Vector3 GetBottomCenterOfBoxCollider()
        {
            return transform.position +
                   new Vector3(BoxCollider.offset.x, BoxCollider.offset.y - BoxCollider.size.y / 2f);
        }

        public Vector3 GetCenterOfBoxCollider()
        {
            return transform.position +
                   new Vector3(BoxCollider.offset.x, BoxCollider.offset.y);
        }

        public void SetVelocity(Vector3 newVelocity)
        {
            _velocity = newVelocity;
        }
    }
}