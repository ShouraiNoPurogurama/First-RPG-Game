using Stats;
using UnityEngine;

public class MagicEnergyController : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private int damage;

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove = true;
    [SerializeField] private bool flipped;
    
    private CharacterStats _myStats;

    public void SetupMagicEnergy(float speed, CharacterStats stats)
    {
        xVelocity = speed;
        _myStats = stats;
    }
    
    private void Update()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            _myStats.DoMagicalDamage(collision.GetComponent<CharacterStats>());
            Destroy(gameObject);
        }

    }
}