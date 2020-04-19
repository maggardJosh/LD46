using UnityEngine;

namespace Pickups
{
    public class Pickup : MonoBehaviour
    {
        public Sprite pickupSprite;

        public PickupType pickupType;
        
        public enum PickupType
        {
            None,
            DoubleJump,
            Slam,
            Vine
        }
        
    }
}