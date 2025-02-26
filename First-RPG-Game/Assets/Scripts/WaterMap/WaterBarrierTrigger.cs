using MainCharacter;
using System.Collections;
using UnityEngine;

public class WaterBarrierTrigger : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                StartCoroutine(PauseAnimation(5f));
            }
        }
    }

    private IEnumerator PauseAnimation(float delay)
    {
        animator.enabled = false; // Tắt animator để dừng animation
        yield return new WaitForSeconds(delay); // Đợi 1.5 giây
        animator.enabled = true; // Bật lại animator để animation tiếp tục
    }
}
