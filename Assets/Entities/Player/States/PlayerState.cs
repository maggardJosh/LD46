namespace Entities.Player.States
{
    public abstract class PlayerState
    {
        protected PlayerController Controller;

        protected PlayerState(PlayerController controller)
        {
            Controller = controller;
        }

        public abstract void HandleUpdate();
        public abstract void HandleFixedUpdate();


        public void OnExitState()
        {
            HandleExit();
        }
        protected virtual void HandleExit() { }

        public void OnEnterState()
        {
            HandleEnter();
        }

        protected virtual void HandleEnter() { }
    }
}