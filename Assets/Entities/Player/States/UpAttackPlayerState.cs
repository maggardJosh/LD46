using UnityEngine;

namespace Entities.Player.States
{
    public class UpAttackPlayerState : PlayerState
    {
        public UpAttackPlayerState(PlayerController controller) : base(controller)
        {
        }

        protected override void HandleEnter()
        {
            Controller.AnimController.SetUpAttack(true);
        }

        protected override void HandleExit()
        {
            Controller.AnimController.SetUpAttack(false);
        }
        
        protected override void HandleUpdateInternal()
        {
            
        }

        protected override void HandleFixedUpdateInternal()
        {
           if(Controller.AnimController.InAnimation("player_upAttack") && Controller.AnimController.AnimationDone())
               Controller.SetPlayerState(new DefaultPlayerState(Controller));
               
        }
    }
}