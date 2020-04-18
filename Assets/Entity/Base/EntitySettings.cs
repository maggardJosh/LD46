using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.Base
{
    [CreateAssetMenu(fileName = "EntitySettings", menuName = "Custom/Entity Setting")]
    public class EntitySettings : ScriptableObject
    {
        [Range(-1, 0)]
        public float XBounceValue = .5f;
        [Range(-1, 0)]
        public float YBounceValue = -.5f;
        
        public float MaxXVel = 5;
        public float MinYVel = -5;
        public float MaxYVel = 10;
        [Range(0, 1)]
        public float GroundFriction = .7f;
        [Range(0,1)]
        public float Friction = 1;
        public bool ObeysGravity = true;
        public LayerMask CollideMask;

        public LayerMask OneWayMask;

        public float GravityMultiplier = 1f;
    }
}