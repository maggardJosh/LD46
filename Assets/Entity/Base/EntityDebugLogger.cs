using UnityEngine;

namespace Entity.Base
{
    public class EntityDebugLogger : IDebugLogger
    {
        private readonly BaseEntity _entity;
        public EntityDebugLogger(BaseEntity e)
        {
            _entity = e;
        }
        public void DebugString(string message)
        {
            if (_entity.debugEntity)
                Debug.Log(message);
        }

        public void DebugLine(Vector2 origin, Vector2 dest, Color color)
        {
            if (!_entity.debugEntity)
                return;
            Debug.DrawLine(origin, dest, color);

        }
    }
}