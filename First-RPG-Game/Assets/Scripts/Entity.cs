using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int FacingDir { get; private set; } = 1;
    private bool _isFacingRight = true;

    [Header("Knock back info")]
    [SerializeField] protected Vector2 knockBackDirection;

    [SerializeField] protected float knockBackDuration;
    protected bool IsKnocked;

    [Header("Collision info")]
    public Transform attackCheck;

    public float attackCheckRadius;

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    #region Components

    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public EntityFX FX { get; private set; }

    #endregion

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        FX = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
    }

    public virtual void Damage()
    {
        FX.StartCoroutine("FlashFX");
        StartCoroutine(nameof(HitKnockBack));
        Debug.Log(gameObject.name + " was damaged!");
    }

    protected virtual IEnumerator HitKnockBack()
    {
        IsKnocked = true;

        Rb.linearVelocity = new Vector2(knockBackDirection.x * -FacingDir, knockBackDirection.y);

        yield return new WaitForSeconds(knockBackDuration);

        IsKnocked = false;
    }

    #region Collisions

    public virtual bool IsGroundDetected()
        => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected()
        => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(groundCheck.position.x + wallCheckDistance * FacingDir, wallCheck.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    #region Flip

    protected virtual void FlipController(float xVelocity)
    {
        if (xVelocity > 0 && !_isFacingRight)
        {
            Flip();
        }
        else if (xVelocity < 0 && _isFacingRight)
        {
            Flip();
        }
    }

    public virtual void Flip()
    {
        FacingDir *= -1;
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    #endregion

    public void SetZeroVelocity()
    {
        if (IsKnocked) 
            return;
        Rb.linearVelocity = new Vector2(0, 0);
    }


    /// <summary>
    /// Called by PlayerMoveState to make movement for Player
    /// </summary>
    /// <remarks><see cref="Entity.FacingDir"/> can only be change through this message.</remarks>
    /// <param name="xVelocity">Velocity for x axis</param>
    /// <param name="yVelocity">Velocity for y axis</param>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
}