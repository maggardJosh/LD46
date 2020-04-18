using UnityEngine;

namespace Entities.Slime
{
    public class JumpSlimeState : SlimeState
    {
        public JumpSlimeState(SlimeController controller) : base(controller)
        {
            float xSpeed = Controller.settings.xJumpSpeed;
            if (Random.Range(0, 2) == 1)
                xSpeed *= -1;
            controller.Entity.SetVelocity(new Vector3(xSpeed, Controller.settings.yJumpSpeed, 0));
        }

        private bool hasLeftGround = false;

        public override void FixedUpdate()
        {
            if (!hasLeftGround)
            {
                if (!Controller.Entity.LastHitResult.HitDown)
                    hasLeftGround = true;
                return;
            }

            if (Controller.Entity.LastHitResult.HitDown)
                Controller.SetState(new IdleSlimeState(Controller));
        }
    }
}