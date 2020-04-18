using UnityEngine;

namespace Entities.Slime
{
    class IdleSlimeState : SlimeState
    {
        private readonly float timeUntilJump;

        public IdleSlimeState(SlimeController controller) : base(controller)
        {
            timeUntilJump = Random.Range(Controller.settings.minJumpTime, Controller.settings.maxJumpTime);
        }

        private float count = 0;

        public override void FixedUpdate()
        {
            count += Time.fixedDeltaTime;
            if (count >= timeUntilJump)
                Controller.SetState(new JumpSlimeState(Controller));
        }
    }
}