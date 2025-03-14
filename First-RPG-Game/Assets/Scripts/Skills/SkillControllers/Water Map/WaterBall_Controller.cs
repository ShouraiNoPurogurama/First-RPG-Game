using Stats;
using UnityEngine;

public class WaterBall_Controller : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    [SerializeField] private float whirlpoolOffsetY;

    private CharacterStats myStats;

    private void Update()
    {
        rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
    }

    public void SetupWaterBall(float _speed, CharacterStats _myStats)
    {
        xVelocity = _speed;
        myStats = _myStats;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //collision.GetComponent<CharacterStats>()?.TakeDamage(damage);
            myStats.DoDamage(collision.GetComponent<CharacterStats>());
            Transform playerTransform = collision.transform;
            WhirlpoolSkill.Instance.SpawnWhirlpool(playerTransform, whirlpoolOffsetY);
            Destroy(gameObject);
        }
    }

    public void FlipWaterBall()
    {
        if (flipped)
        {
            return;
        }
        xVelocity *= -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
}
