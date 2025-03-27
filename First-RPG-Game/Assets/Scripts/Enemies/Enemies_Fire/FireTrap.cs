using System.Collections;
using MainCharacter;
using UnityEngine;

namespace Enemies.Enemies_Fire
{
    public class FireTrap : MonoBehaviour
    {
        public int trapdamage = 30;
        void Start()
        {

        }
        void Update()
        {

        }

        private void DealDamage()
        {
            Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.5f, 1.5f), 0);
            foreach (Collider2D playerCollider in hitPlayers)
            {
                Player player = playerCollider.GetComponent<Player>();
                if (player != null)
                {
                    player.Stats.TakeDamageNoImpact(trapdamage, Color.yellow);
                }
            }

        }
    }

}
