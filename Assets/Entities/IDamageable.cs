using UnityEngine;

namespace Entities
{
    public interface IDamageable
    {
        void TakeDamage(MonoBehaviour damager);
    }
}