using System.Collections.Generic;
using Pickups;
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

            if (HandlePickupLogic())
                return;

            if (Controller.CanSlam && HandleSlamLogic())
                return;
            if (HandleAttackLogic())
                return;
            
            UpdateAnimator();
                
        }

        private bool HandlePickupLogic()
        {
            if (Controller.CarryingPickup)
            {
                CheckHomeCollisionForPickup();
                return false;
            }
            
            if (!Controller.Entity.LastHitResult.HitDown)
                return false;
            
            bool shouldAttackAccordingToInput =
                (Controller.CurrentInput.VineInput && Controller._attackBuffered < Controller.settings.attackBufferLength);
            if (!shouldAttackAccordingToInput) 
                return false;
            
            List<Collider2D> results = new List<Collider2D>();
            int numHits = Controller.Entity.BoxCollider.OverlapCollider(
                new ContactFilter2D {useLayerMask = true, layerMask = Controller.settings.pickupLayer}, results);
            if (numHits == 0)
                return false;

            foreach (var r in results)
            {
                var pickup = r.GetComponent<Pickup>();
                if (pickup == null)
                    continue;
                if (!Controller.CanPickup(pickup.pickupType))
                    continue;
                Controller.Pickup(pickup);
                Controller._attackBuffered = Controller.settings.attackBufferLength;
                return true;
            }

            return false;
        }

        private void CheckHomeCollisionForPickup()
        {
            List<Collider2D> results = new List<Collider2D>();
            int numHits = Controller.Entity.BoxCollider.OverlapCollider(
                new ContactFilter2D {useLayerMask = true, layerMask = Controller.settings.homeLayer}, results);
            if (numHits > 0)
                Controller.ApplyPickup();

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

        private bool HandleAttackLogic()
        {
            
            
            if (!Controller.Entity.LastHitResult.HitDown)
                return false;
            
            bool shouldAttackAccordingToInput =
                (Controller.CurrentInput.VineInput && Controller._attackBuffered < Controller.settings.attackBufferLength);
            
            if (!shouldAttackAccordingToInput)
                return false;

            Controller._attackBuffered = Controller.settings.attackBufferLength;
            
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
                else if (Controller.CanDoubleJump && !_hasDoubleJumped)
                {
                    _hasDoubleJumped = true;
                    Jump();   
                    TutorialManager.Instance.SetTutorialStep(TutorialManager.TutorialStep.DoubleJumpDone);
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