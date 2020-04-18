namespace Entities.Player
{
    public class PlayerInput
    {
        public float XInput = 0;
        public float YInput = 0;
        public bool JumpInput = false;
        public override string ToString()
        {
            return $"X:{XInput:F} Y:{YInput:F} Jump:{(JumpInput ? "1" : "0")}";
        }
    }
}