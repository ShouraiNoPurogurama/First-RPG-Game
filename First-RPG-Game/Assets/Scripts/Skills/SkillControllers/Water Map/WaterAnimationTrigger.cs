using Stats;
using UnityEngine;

namespace Skills.SkillControllers.Water_Map
{
    public class WaterAnimationTrigger : MonoBehaviour
    {
        private Animator anim;

        [SerializeField] private int damage = 1;
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.GetComponent<CharacterStats>()?.TakeDamage(damage);
        }
        public void OnWhirlpoolAnimation()
        {
            anim.SetBool("isWaterPushing", true);
            Collider2D coll = GetComponent<Collider2D>();
            if (coll != null)
            {
                coll.enabled = true;
            }
        }
        public void OffWhirlpoolAnimation()
        {
            anim.SetBool("isWaterPushing", false);
            Collider2D coll = GetComponent<Collider2D>();
            if (coll != null)
            {
                coll.enabled = false;
            }
            Destroy(gameObject);
        }
    }
}
