using UnityEngine;

namespace Entities.Slime
{
    [CreateAssetMenu(menuName = "Settings/Slime")]
    public class SlimeSettings : ScriptableObject
    {
        public float xJumpSpeed = 2;
        public float yJumpSpeed = 5;
        public float minJumpTime = 3;
        public float maxJumpTime = 5;
        public LayerMask damageLayer;
    }
}