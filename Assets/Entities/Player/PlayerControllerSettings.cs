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
        public GameObject shovelGrapplePrefab;
        public GameObject shovelGrappleUpPrefab;
        public float slamSpeed = -10;
        public LayerMask pickupLayer;
        public LayerMask homeLayer;
        public LayerMask damageLayer;
        public float stunTime  = 1f;
        public float stunVelocityStrength = 10f;
        public GameObject PickupDiscardPrefab;
        public float attackBufferLength = .2f;
        public float InvulnerabilityTime = 3f;
    }
}