namespace Entities.Slime
{
    public abstract class SlimeState
    {
        protected readonly SlimeController Controller;

        protected SlimeState(SlimeController controller)
        {
            Controller = controller;
        }

        public abstract void FixedUpdate();
    }
}