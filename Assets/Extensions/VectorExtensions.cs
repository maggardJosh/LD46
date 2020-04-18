using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions 
    {
        public static Vector3 ToVector3(this Vector2 vect)
        {
            return new Vector3(vect.x, vect.y);
        }

        public static Vector2 ToVector2(this Vector3 vect)
        {
            return new Vector2(vect.x, vect.y);
        }

        public static Vector3 Flip(this Vector3 vector)
        {
            return new Vector3(vector.x *  -1, vector.y);
        }

        public static Vector3Int ToVector3Int(this Vector3 vector)
        {
            return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), 0);
        }
    }
}
