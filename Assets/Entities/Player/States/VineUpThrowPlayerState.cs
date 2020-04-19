using Extensions;
using UnityEngine;

namespace Entities.Player.States
{
    public class VineUpThrowPlayerState : PlayerState
    {
        private Vector3 _startingVelocity;
        private ShovelGrappleHook _hook;

        private Vector3 pos;
        public VineUpThrowPlayerState(PlayerController controller) : base(controller)
        {
            _startingVelocity = controller.Entity.GetVelocity();
            pos = controller.transform.position;

        }

        protected override void HandleEnter()
        {
            TutorialManager.Instance.SetTutorialStep(TutorialManager.TutorialStep.VineDone);
            Controller.AnimController.SetVineUpThrow(true);
        }

        protected override void HandleExit(PlayerState nextState)
        {
            Controller.AnimController.SetVineUpThrow(false);
            if(!(nextState is VineUpRetrievePlayerState) && _hook != null)
                Object.Destroy(_hook.gameObject);
        }

        protected override void HandleUpdateInternal()
        {
            Controller.Entity.SetVelocity(Vector3.zero);
            Controller.transform.position = pos;
        }

        private bool _hasEnteredThrowAnim = false;
        protected override void HandleFixedUpdateInternal()
        {
            if (!_hasEnteredThrowAnim)
            {
                if (Controller.AnimController.InAnimation("player_vineUpThrow"))
                    _hasEnteredThrowAnim = true;
                return;
            }

            if (_hook == null && Controller.AnimController.AnimationDone())
            {
                var offset = Controller.grappleHookUpAnchor.localPosition;
                if (Controller.SRend.flipX)
                    offset = offset.Flip();
                var hook = Object.Instantiate(Controller.settings.shovelGrappleUpPrefab,
                    Controller.transform.position + offset,
                    Quaternion.identity);
                _hook = hook.GetComponent<ShovelGrappleHook>();
                _hook.OnReturnHook += hitSomething =>
                    Controller.SetPlayerState(new VineUpRetrievePlayerState(Controller, !hitSomething, _hook));
                _hook.ShootGrappleUp(Controller);
            }
        }
    }
}