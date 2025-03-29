using System.Collections;
using Stats;
using UnityEngine;

namespace Skills.SkillControllers.Water_Map
{
    public class WaterBarrierTrigger : MonoBehaviour
    {
        private Animator anim;
        [SerializeField] private float waterInterval = 5f;
        [SerializeField] private float waterDuration = 1f;
        [SerializeField] private int damage = 20;
        private void Start()
        {
            anim = GetComponent<Animator>();
            StartCoroutine(WaterColumnRoutine());
        }

        private IEnumerator WaterColumnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(waterInterval);
                anim.SetBool("isWaterPulse", true);
                yield return new WaitForSeconds(waterDuration);
                anim.SetBool("isWaterPulse", false);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.GetComponent<CharacterStats>()?.TakeDamage(damage);
        }
    }
}
