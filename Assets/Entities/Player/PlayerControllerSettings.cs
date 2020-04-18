using UnityEngine;

namespace Entities.Player
{
    [CreateAssetMenu(menuName = "Settings/PlayerController")]
    public class PlayerControllerSettings : ScriptableObject
    {
        public float speed = 5f;
        public float jumpStrength = 10f;
        public float allowedTimeSinceGroundedToJump = .3f;
        [Range(0,1)]
        public float jumpLetGoMultiplier = .7f;
        public float jumpBufferLength = .2f;
    }
}