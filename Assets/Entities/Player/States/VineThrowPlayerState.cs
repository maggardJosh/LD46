using UnityEngine;

namespace Entities.Player.States
{
    public class VineThrowPlayerState : PlayerState
    {
        private readonly Vector2 _direction;
        private Vector3 _startingVelocity;

        public VineThrowPlayerState(PlayerController controller, Vector2 direction) : base(controller)
        {
            _direction = direction;
            _startingVelocity = controller.Entity.GetVelocity();
            
        }

        protected override void HandleEnter()
        {
            Controller.AnimController.SetVineThrow(true);
        }

        protected override void HandleExit()
        {
            Controller.AnimController.SetVineThrow(false);
        }

        public override void HandleUpdate()
        {
            Controller.Entity.SetVelocity(Vector3.zero);
        }

        private bool _hasEnteredThrowAnim = false;
        public override void HandleFixedUpdate()
        {
            if (!_hasEnteredThrowAnim)
            {
                if (Controller.AnimController.InAnimation("player_vineThrow"))
                    _hasEnteredThrowAnim = true;
                return;
            }

            if (Controller.AnimController.AnimationDone())
            {
                //TODO: Actually throw vine shovel here
                Controller.SetPlayerState(new VineRetrievePlayerState(Controller, true));
            }
        }
    }
}