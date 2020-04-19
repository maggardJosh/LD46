﻿using UnityEngine;

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
            Controller.AnimController.SetStun(true);
        }

        protected override void HandleExit()
        {
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