using UnityEngine;

namespace Entities.Player.States
{
    public class DefaultPlayerState : PlayerState
    {
        public DefaultPlayerState(PlayerController controller) : base(controller) { }

        private float _timeSinceGrounded = float.MaxValue;
        private float _jumpBuffered = float.MaxValue;
            
        public override void HandleUpdate()
        {
            if (Controller.CurrentInput.JumpInput && !Controller.LastInput.JumpInput)
                _jumpBuffered = 0;
        }

        public override void HandleFixedUpdate()
        {
            HandleXInput(Controller.CurrentInput.XInput);
                
            HandleJumpLogic();

            if (HandleVineLogic())
                return;
            
            UpdateAnimator();
                
        }

        private bool HandleVineLogic()
        {
            if (!Controller.CurrentInput.VineInput || Controller.LastInput.VineInput) 
                return false;
            
            Controller.SetPlayerState(new VineThrowPlayerState(Controller, Controller.SRend.flipX ? Vector2.left : Vector2.right));
            return true;

        }


        private void UpdateAnimator()
        {
            bool grounded = Controller.Entity.LastHitResult.HitDown;
            if (grounded && Controller.Entity.GetYVelocity() <= 0)
                _timeSinceGrounded = 0;
            Controller.AnimController.SetGrounded(grounded);
            Controller.AnimController.SetXMoving(Mathf.Abs(Controller.CurrentInput.XInput) > 0f);
            Controller.AnimController.SetYSpeed(Controller.Entity.GetYVelocity());
        }

        private void Jump()
        {
            _timeSinceGrounded = Controller.settings.allowedTimeSinceGroundedToJump;
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

            if (_timeSinceGrounded < Controller.settings.allowedTimeSinceGroundedToJump &&
                (Controller.CurrentInput.JumpInput && _jumpBuffered < Controller.settings.jumpBufferLength))
                Jump();
                
            if(!Controller.CurrentInput.JumpInput && Controller.Entity.GetYVelocity() > 0)
                Controller.Entity.SetYVelocity(Controller.Entity.GetYVelocity()*Controller.settings.jumpLetGoMultiplier);
                
            if (_timeSinceGrounded < Controller.settings.allowedTimeSinceGroundedToJump)
                _timeSinceGrounded += Time.fixedDeltaTime;
                
            if (_jumpBuffered < Controller.settings.jumpBufferLength)
                _jumpBuffered += Time.fixedDeltaTime;
        }
    }
}