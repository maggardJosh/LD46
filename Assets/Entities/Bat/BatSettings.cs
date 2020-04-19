using UnityEngine;

namespace Entities.Bat
{
    [CreateAssetMenu(menuName = "Settings/Bat")]
    public class BatSettings : ScriptableObject
    {
        public float speed = 5f;
        public float damageBounceValue = 5f;
    }
}