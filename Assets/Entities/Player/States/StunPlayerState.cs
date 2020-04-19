using UnityEngine;

namespace Entities.Player.States
{
    public class StunPlayerState : PlayerState
    {
        public StunPlayerState(PlayerController controller, Vector3 damageVect) : base(controller)
        {
            Controller.Entity.SetVelocity(damageVect);
        }

        protected override void HandleEnter()
        {
            Controller.Entity.settings = Controller.stunSettings;
            Controller.AnimController.SetStun(true);
        }

        protected override void HandleExit(PlayerState nextState)
        {
            Controller.Entity.settings = Controller.normalSettings;
            Controller.AnimController.SetStun(false);
        }
        
        protected override void HandleUpdateInternal()
        {
            
        }
        
        protected override void HandleFixedUpdateInternal()
        {
            if(timeInState >= Controller.settings.stunTime)
            {
                Controller.SetPlayerState(new DefaultPlayerState(Controller));
                return;
            }
        }
    }
}