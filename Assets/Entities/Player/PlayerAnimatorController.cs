﻿using UnityEngine;

namespace Entities.Player
{
    public class PlayerAnimatorController
    {
        private readonly Animator _animator;
        private static readonly int XMoving = Animator.StringToHash("XMoving");
        private static readonly int YSpeed = Animator.StringToHash("YSpeed");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int VineThrow = Animator.StringToHash("VineThrow");
        private static readonly int VineRetrieve = Animator.StringToHash("VineRetrieve");
        private static readonly int VineUpThrow = Animator.StringToHash("VineUpThrow");
        private static readonly int VineUpRetrieve = Animator.StringToHash("VineUpRetrieve");
        private static readonly int Slam = Animator.StringToHash("Slam");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int UpAttack = Animator.StringToHash("UpAttack");
        private static readonly int Stun = Animator.StringToHash("Stun");

        public PlayerAnimatorController(Animator animator)
        {
            _animator = animator;
        }
        
        public bool InAnimation(string animName)
        {
            return _animator.GetCurrentAnimatorStateInfo(0).IsName(animName);
        }

        public bool AnimationDone()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
        }

        public void SetXMoving(bool xMoving)
        {
            _animator.SetBool(XMoving, xMoving);
        }

        public void SetYSpeed(float ySpeed)
        {
            _animator.SetFloat(YSpeed, ySpeed);
        }

        public void SetGrounded(bool grounded)
        {
            _animator.SetBool(Grounded, grounded);
        }

        public void SetVineThrow(bool vineThrow)
        {
            _animator.SetBool(VineThrow, vineThrow);
        }

        public void SetVineRetrieve(bool vineRetrieve)
        {
            _animator.SetBool(VineRetrieve, vineRetrieve);
        }
        
        public void SetVineUpThrow(bool vineThrow)
        {
            _animator.SetBool(VineUpThrow, vineThrow);
        }

        public void SetVineUpRetrieve(bool vineRetrieve)
        {
            _animator.SetBool(VineUpRetrieve, vineRetrieve);
        }

        public void SetSlam(bool slamValue)
        {
            _animator.SetBool(Slam, slamValue);
        }
        public void SetAttack(bool attack)
        {
            _animator.SetBool(Attack, attack);
        }
        public void SetUpAttack(bool upAttack)
        {
            _animator.SetBool(UpAttack, upAttack);
        }

        public void SetStun(bool stun)
        {
            _animator.SetBool(Stun, stun);
        }
    }
}