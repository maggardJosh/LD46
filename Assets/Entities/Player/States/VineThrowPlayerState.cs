using Extensions;
using UnityEngine;

namespace Entities.Player.States
{
    public class VineThrowPlayerState : PlayerState
    {
        private readonly Vector2 _direction;
        private Vector3 _startingVelocity;
        private ShovelGrappleHook _hook;

        private Vector3 pos;
        public VineThrowPlayerState(PlayerController controller, Vector2 direction) : base(controller)
        {
            _direction = direction;
            _startingVelocity = controller.Entity.GetVelocity();
            pos = controller.transform.position;

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
            Controller.transform.position = pos;
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

            if (_hook == null && Controller.AnimController.AnimationDone())
            {
                var hookOffset = Controller.grappleHookAnchor.transform.localPosition;
                if (Controller.SRend.flipX)
                    hookOffset = hookOffset.Flip();
                var hook = Object.Instantiate(Controller.settings.shovelGrapplePrefab,
                    Controller.transform.position +  hookOffset,
                    Quaternion.Euler(new Vector3(0, 0, Controller.SRend.flipX ? 180 : 0)));
                _hook = hook.GetComponent<ShovelGrappleHook>();
                _hook.OnReturnHook += hitSomething =>
                    Controller.SetPlayerState(new VineRetrievePlayerState(Controller, !hitSomething, _hook));
                _hook.ShootGrapple(Controller);
            }
        }
    }
}