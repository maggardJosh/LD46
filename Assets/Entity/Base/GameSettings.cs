using UnityEngine;

namespace Entity.Base
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Custom/Game Setting")]
    public class GameSettings : ScriptableObject
    {
        public float tileSize = 16;
        public float collisionOffsetValue = .1f;
        public float gravity = 1f;

        private static GameSettings _instance;
        private static GameSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<GameSettings>("GameSettings");
                return _instance;
            }
        }
        public static float TileSize { get => Instance.tileSize; }
        public static float CollisionOffsetValue { get => Instance.collisionOffsetValue; }
        public static float Gravity { get => Instance.gravity; }
    }
}