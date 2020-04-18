namespace Entities.Player.States
{
    public class VineRetrievePlayerState : PlayerState
    {
        private readonly bool _returnVineToPlayer;

        public VineRetrievePlayerState(PlayerController controller, bool returnVineToPlayer) : base(controller)
        {
            _returnVineToPlayer = returnVineToPlayer;
        }

        protected override void HandleEnter()
        {
            Controller.AnimController.SetVineRetrieve(true);
        }

        protected override void HandleExit()
        {
            Controller.AnimController.SetVineRetrieve(false);
        }
        
        public override void HandleUpdate()
        {
            
        }

        public override void HandleFixedUpdate()
        {
            
        }
    }
}