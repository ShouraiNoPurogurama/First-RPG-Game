using MainCharacter;
using System.Collections;
using UnityEngine;

public class WaterBarrierTrigger : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float waterInterval = 5f;
    [SerializeField] private float waterDuration = 1f;
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
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Stats.TakeDamage(20);
        }

    }
}
