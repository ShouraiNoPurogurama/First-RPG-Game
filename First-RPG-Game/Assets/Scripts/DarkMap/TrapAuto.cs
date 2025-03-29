using MainCharacter;
using Stats;
using UnityEngine;

namespace DarkMap
{
    public class TrapAuto : MonoBehaviour
    {
        [SerializeField] private int damage;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>())
            {
                CharacterStats targetStats = collision.GetComponent<CharacterStats>();
                if (targetStats != null)
                {
                    targetStats.TakeDamage(damage);
                }
            }
        }
    
    }
}
