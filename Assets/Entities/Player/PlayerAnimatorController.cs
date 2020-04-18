using UnityEngine;

namespace Entities.Player
{
    public class PlayerAnimatorController
    {
        private readonly Animator _animator;
        private static readonly int XMoving = Animator.StringToHash("XMoving");
        private static readonly int YSpeed = Animator.StringToHash("YSpeed");
        private static readonly int Grounded = Animator.StringToHash("Grounded");

        public PlayerAnimatorController(Animator animator)
        {
            _animator = animator;
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
    }
}