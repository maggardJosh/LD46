using UnityEngine;

namespace Entities.Player.States
{
    public class VineRetrievePlayerState : PlayerState
    {
        private readonly bool _returnVineToPlayer;
        private readonly ShovelGrappleHook _hook;
        private Vector3 pos;

        public VineRetrievePlayerState(PlayerController controller, bool returnVineToPlayer, ShovelGrappleHook hook) : base(controller)
        {
            _returnVineToPlayer = returnVineToPlayer;
            _hook = hook;
            pos = controller.transform.position;

        }

        protected override void HandleEnter()
        {
            Controller.AnimController.SetVineRetrieve(true);
        }

        protected override void HandleExit()
        {
            Controller.AnimController.SetVineRetrieve(false);
        }
        
        protected override void HandleUpdateInternal()
        {
            Controller.Entity.SetVelocity(Vector3.zero);
        }
        
        protected override void HandleFixedUpdateInternal()
        {
            var diff = (_hook.transform.position - Controller.transform.position).x > 0 ? Vector3.left : Vector3.right;
            if (_returnVineToPlayer)
            {
                _hook.transform.position += diff * _hook.speed * Time.fixedDeltaTime;
                Controller.transform.position = pos;
            }
            else
            {
                Controller.transform.position -= diff * _hook.speed * Time.fixedDeltaTime;
            }

            if (Mathf.Abs((_hook.transform.position - Controller.transform.position).x) < .2f)
            {
                Controller.SetPlayerState(new DefaultPlayerState(Controller));
                Object.Destroy(_hook.gameObject);
            }
        }
    }
}