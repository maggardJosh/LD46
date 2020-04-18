using UnityEngine;

namespace Entity.Base
{
    public interface IDebugLogger
    {
        void DebugString(string message);
        void DebugLine(Vector2 origin, Vector2 dest, Color color);
    }
}