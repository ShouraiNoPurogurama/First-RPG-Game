using MainCharacter;
using UnityEngine;

namespace Enemies.Enemies_Fire
{
    public class FireBall : MonoBehaviour
    {

        void Start()
        {
       
        }
        void Update()
        {
        
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Stats.TakeDamageNoImpact(80,Color.yellow);
            }

        }
    }
}
