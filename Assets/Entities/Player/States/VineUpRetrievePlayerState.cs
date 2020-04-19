using UnityEngine;

namespace Entities.Player.States
{
    public class VineUpRetrievePlayerState : PlayerState
    {
        private readonly bool _returnVineToPlayer;
        private readonly ShovelGrappleHook _hook;
        private Vector3 pos;

        public VineUpRetrievePlayerState(PlayerController controller, bool returnVineToPlayer, ShovelGrappleHook hook) : base(controller)
        {
            _returnVineToPlayer = returnVineToPlayer;
            _hook = hook;
            pos = controller.transform.position;

        }

        protected override void HandleEnter()
        {
            Controller.AnimController.SetVineUpRetrieve(true);
            Controller.Entity.enabled = false;
        }

        protected override void HandleExit(PlayerState nextState)
        {
            Controller.AnimController.SetVineUpRetrieve(false);
            Controller.Entity.enabled = true;
            Object.Destroy(_hook.gameObject);
        }
        
        protected override void HandleUpdateInternal()
        {
            Controller.Entity.SetVelocity(Vector3.zero);
        }
        
        protected override void HandleFixedUpdateInternal()
        {
            var diff = Vector3.down;
            if (_returnVineToPlayer)
            {
                _hook.transform.position += diff * _hook.speed * Time.fixedDeltaTime;
                Controller.transform.position = pos;
            }
            else
            {
                Controller.transform.position -= diff * _hook.speed * Time.fixedDeltaTime;
            }

            if (Mathf.Abs((_hook.transform.position - Controller.transform.position).y) < .4f)
            {
                Controller.SetPlayerState(new DefaultPlayerState(Controller));
            }
        }
    }
}