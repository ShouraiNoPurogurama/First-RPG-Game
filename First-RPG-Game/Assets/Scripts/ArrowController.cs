using System;
using Stats;
using UnityEditor;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private int damage;

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove = true;
    [SerializeField] private bool flipped;
    
    private CharacterStats _myStats;

    public void SetupArrow(float speed, CharacterStats stats)
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
            _myStats.DoDamage(collision.GetComponent<CharacterStats>());
            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        
        Destroy(gameObject, 5);
    }

    public void FlipArrow()
    {
        if (flipped) return;

        flipped = true;
        targetLayerName = "Enemy";
        xVelocity = -xVelocity;
        transform.Rotate(0, 180, 0);
    }
}