using System.Collections.Generic;

namespace Entity.Base
{
    public class BaseEntityManager
    {
        private readonly List<BaseEntity> _baseEntities = new List<BaseEntity>();
        public static BaseEntityManager Instance => _instance ?? (_instance = new BaseEntityManager());

        private static BaseEntityManager _instance;

        public void AddEntity(BaseEntity newEntity)
        {
            if (_baseEntities.Contains(newEntity))
                return;
            _baseEntities.Add(newEntity);
        }

        public void RemoveEntity(BaseEntity entityToRemove)
        {
            if (!_baseEntities.Contains(entityToRemove))
                return;
            _baseEntities.Remove(entityToRemove);
        }

        public IEnumerable<BaseEntity> Entities => _baseEntities;
    }
}