using UnityEngine;

namespace Entities.Player.States
{
    public class AttackPlayerState : PlayerState
    {
        public AttackPlayerState(PlayerController controller) : base(controller)
        {
        }

        protected override void HandleEnter()
        {
            if(TutorialManager.Instance.CurrentStep == TutorialManager.TutorialStep.Attack)
                TutorialManager.Instance.SetTutorialStep(TutorialManager.TutorialStep.AttackDone);
            Controller.AnimController.SetAttack(true);
        }

        protected override void HandleExit(PlayerState nextState)
        {
            Controller.AnimController.SetAttack(false);
        }
        
        protected override void HandleUpdateInternal()
        {
            
        }

        protected override void HandleFixedUpdateInternal()
        {
           if(Controller.AnimController.InAnimation("player_attack") && Controller.AnimController.AnimationDone())
               Controller.SetPlayerState(new DefaultPlayerState(Controller));
               
        }
    }
}