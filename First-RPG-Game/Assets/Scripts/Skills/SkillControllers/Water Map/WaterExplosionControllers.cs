using Stats;
using UnityEngine;

public class WaterExplosionSkill : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float explosionRadius = 2f;

    public void ExplosionEventDamage()
    {
        Debug.Log("ExplosionEventDamage");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in colliders)
        {
            PlayerStats stats = col.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }
    }
    public void OnDestroyGameObject()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
