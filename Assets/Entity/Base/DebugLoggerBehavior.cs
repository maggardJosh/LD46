using UnityEngine;

namespace Entity.Base
{
    public class DebugLoggerBehavior : MonoBehaviour, IDebugLogger
    {
        public bool debugEnabled = false;
        public void DebugString(string message)
        {
            if (debugEnabled)
                Debug.Log(message);
        }

        public void DebugLine(Vector2 origin, Vector2 dest, Color color)
        {
            if (debugEnabled) 
                Debug.DrawLine(origin, dest, color);
        }
    }
}