
    using System;
    using Entity.Base;
    using UnityEngine;

    namespace Entities.Player
    {
        [RequireComponent(typeof(BaseEntity))]
        [RequireComponent(typeof(Animator))]
        [RequireComponent(typeof(DebugLoggerBehavior))]
        public class PlayerController : MonoBehaviour
        {
            private BaseEntity _entity;
            private PlayerAnimatorController _animController;
            private IPlayerInputProvider _inputProvider;
            private SpriteRenderer _sRend;
            private DebugLoggerBehavior _logger;

            [SerializeField] private PlayerControllerSettings settings;
            
            private void Start()
            {
                _entity = GetComponent<BaseEntity>();
                _animController = new PlayerAnimatorController(GetComponent<Animator>());
                _sRend = GetComponentInChildren<SpriteRenderer>();
                if(_inputProvider == null)
                    _inputProvider = new UnityPlayerInputProvider();
                _logger = GetComponent<DebugLoggerBehavior>();
            }

            private PlayerInput _lastInput = new PlayerInput();
            private PlayerInput _currentInput = new PlayerInput();
            
            private void Update()
            {
                _lastInput = _currentInput;
                _currentInput = _inputProvider.GetInput();
                
                if (_currentInput.JumpInput && !_lastInput.JumpInput)
                    _jumpBuffered = 0;
            }

            private float _timeSinceGrounded = float.MaxValue;
            
            private void FixedUpdate()
            {
                HandleXInput(_currentInput.XInput);
                
                HandleJumpLogic();

                UpdateAnimator();
            }

            private float _jumpBuffered = float.MaxValue;
            private void HandleJumpLogic()
            {

                if (_timeSinceGrounded < settings.allowedTimeSinceGroundedToJump &&
                    (_currentInput.JumpInput && _jumpBuffered < settings.jumpBufferLength))
                    Jump();
                
                if(!_currentInput.JumpInput && _entity.GetYVelocity() > 0)
                    _entity.SetYVelocity(_entity.GetYVelocity()*settings.jumpLetGoMultiplier);
                
                if (_timeSinceGrounded < settings.allowedTimeSinceGroundedToJump)
                    _timeSinceGrounded += Time.fixedDeltaTime;
                
                if (_jumpBuffered < settings.jumpBufferLength)
                    _jumpBuffered += Time.fixedDeltaTime;
            }

            private void UpdateAnimator()
            {
                bool grounded = _entity.LastHitResult.HitDown;
                if (grounded && _entity.GetYVelocity() <= 0)
                    _timeSinceGrounded = 0;
                _animController.SetGrounded(grounded);
                _animController.SetXMoving(Mathf.Abs(_currentInput.XInput) > 0f);
                _animController.SetYSpeed(_entity.GetYVelocity());
            }

            private void Jump()
            {
                _timeSinceGrounded = settings.allowedTimeSinceGroundedToJump;
                _entity.SetYVelocity(settings.jumpStrength);
            }

            private void HandleXInput(float currentInputXInput)
            {
                if (Mathf.Abs(_currentInput.XInput) <= 0)
                    return;
                _sRend.flipX = _currentInput.XInput < 0;
                    
                _entity.SetXVelocity(currentInputXInput * settings.speed);
                
            }
        }
    }
