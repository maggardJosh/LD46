using UnityEngine;

namespace Entities.Player.States
{
    public class DefaultPlayerState : PlayerState
    {
        public DefaultPlayerState(PlayerController controller) : base(controller) { }

        private bool _hasDoubleJumped = false;
        private float _timeSinceGrounded = float.MaxValue;
        private float _jumpBuffered = float.MaxValue;
            
        protected override void HandleUpdateInternal()
        {
            if (Controller.CurrentInput.JumpInput && !Controller.LastInput.JumpInput)
                _jumpBuffered = 0;
        }

        protected override void HandleFixedUpdateInternal()
        {
            HandleXInput(Controller.CurrentInput.XInput);
                
            HandleJumpLogic();

            if (Controller.CanSlam && HandleSlamLogic())
                return;
            if (HandleVineLogic())
                return;
            
            UpdateAnimator();
                
        }

        private bool HandleSlamLogic()
        {
            if (Controller.Entity.LastHitResult.HitDown)
                return false;

            if (Controller.CurrentInput.YInput >= 0)
                return false;
            
            if (!Controller.CurrentInput.VineInput || Controller.LastInput.VineInput) 
                return false;
            
            Controller.SetPlayerState(new SlamPlayerState(Controller));
            return true;
        }

        private bool HandleVineLogic()
        {
            if (!Controller.Entity.LastHitResult.HitDown)
                return false;
            
            if (!Controller.CurrentInput.VineInput || Controller.LastInput.VineInput) 
                return false;

            if (Controller.CurrentInput.YInput > 0)
                Controller.SetPlayerState(GetUpAttackState());
            else
                Controller.SetPlayerState(GetAttackState());
            return true;

        }

        private PlayerState GetUpAttackState()
        {
            if(Controller.CanVine)
                return new VineUpThrowPlayerState(Controller);
            return new UpAttackPlayerState(Controller);
        }

        private PlayerState GetAttackState()
        {
            if (Controller.CanVine)
                return new VineThrowPlayerState(Controller, Controller.SRend.flipX ? Vector2.left : Vector2.right);
            return new AttackPlayerState(Controller);
        }

        private void UpdateAnimator()
        {
            bool grounded = Controller.Entity.LastHitResult.HitDown;
            if (grounded && Controller.Entity.GetYVelocity() <= 0)
            {
                _timeSinceGrounded = 0;
                _hasDoubleJumped = false;
            }

            Controller.AnimController.SetGrounded(grounded);
            Controller.AnimController.SetXMoving(Mathf.Abs(Controller.CurrentInput.XInput) > 0f);
            Controller.AnimController.SetYSpeed(Controller.Entity.GetYVelocity());
        }

        private void Jump()
        {
            _timeSinceGrounded = Controller.settings.allowedTimeSinceGroundedToJump;
            _jumpBuffered = Controller.settings.jumpBufferLength;
            Controller.Entity.SetYVelocity(Controller.settings.jumpStrength);
        }

        private void HandleXInput(float currentInputXInput)
        {
            if (Mathf.Abs(Controller.CurrentInput.XInput) <= 0)
                return;
            Controller.SRend.flipX = Controller.CurrentInput.XInput < 0;
                    
            Controller.Entity.SetXVelocity(currentInputXInput * Controller.settings.speed);
                
        }
            
            
        private void HandleJumpLogic()
        {
            bool shouldJumpAccordingToInput =
                (Controller.CurrentInput.JumpInput && _jumpBuffered < Controller.settings.jumpBufferLength);
            if (shouldJumpAccordingToInput)
            {
                if (_timeSinceGrounded < Controller.settings.allowedTimeSinceGroundedToJump)
                {
                    Jump();
                }
                else if (!_hasDoubleJumped)
                {
                    _hasDoubleJumped = true;
                    Jump();   
                    SpawnDoubleJumpEffect();
                }
            }
                
            if(!Controller.CurrentInput.JumpInput && Controller.Entity.GetYVelocity() > 0)
                Controller.Entity.SetYVelocity(Controller.Entity.GetYVelocity()*Controller.settings.jumpLetGoMultiplier);
                
            if (_timeSinceGrounded < Controller.settings.allowedTimeSinceGroundedToJump)
                _timeSinceGrounded += Time.fixedDeltaTime;
                
            if (_jumpBuffered < Controller.settings.jumpBufferLength)
                _jumpBuffered += Time.fixedDeltaTime;
        }

        private void SpawnDoubleJumpEffect()
        {
            Object.Instantiate(Controller.doubleJumpEffect, Controller.transform.position, Quaternion.identity);
        }
    }
}