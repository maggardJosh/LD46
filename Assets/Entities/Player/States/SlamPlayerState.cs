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
            TutorialManager.Instance.SetTutorialStep(TutorialManager.TutorialStep.SlamDone);
            Controller.AnimController.SetSlam(true);
        }

        protected override void HandleExit(PlayerState nextState)
        {
            Controller.AnimController.SetSlam(false);
        }
        
        protected override void HandleUpdateInternal()
        {
            
        }

        private float touchGroundCount = 0;
        private bool playedSound = false;
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
                if(!playedSound)
                {
                    AudioManager.PlayOneShot(AudioClips.Instance.DestroyTile);
                    playedSound = true;
                }
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