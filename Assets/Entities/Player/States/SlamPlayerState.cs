using UnityEngine;

namespace Entities.Player.States
{
    public class SlamPlayerState : PlayerState
    {
        public SlamPlayerState(PlayerController controller) : base(controller)
        {
        }

        protected override void HandleEnter()
        {
            Controller.AnimController.SetSlam(true);
        }

        protected override void HandleExit()
        {
            Controller.AnimController.SetSlam(false);
        }
        
        protected override void HandleUpdateInternal()
        {
            
        }

        private float touchGroundCount = 0;
        protected override void HandleFixedUpdateInternal()
        {
            if(timeInState < .15f)
            {
                Controller.Entity.SetVelocity(Vector3.zero);
                return;
            }
            Controller.Entity.SetVelocity(new Vector3(0,Controller.settings.slamSpeed, 0));
            if (Controller.Entity.LastHitResult.HitDown)
            {
                foreach (var f in Controller.Entity.LastHitResult.VerticalHits)
                {
                    var breakableTile = f.collider.GetComponent<BreakableTile>();
                    if (breakableTile != null)
                    {
                        Object.Destroy(breakableTile.gameObject);
                    }
                }
                touchGroundCount += Time.fixedDeltaTime;
                if(touchGroundCount > .2f)
                    Controller.SetPlayerState(new DefaultPlayerState(Controller));
            }
        }
    }
}