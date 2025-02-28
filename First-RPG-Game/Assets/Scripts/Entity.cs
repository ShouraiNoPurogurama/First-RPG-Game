using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int FacingDir { get; private set; } = 1;
    private bool _isFacingRight = true;

    public System.Action onFlipped;

    public bool IsBusy { get; private set; }
    
    public SpriteRenderer Sr { get; private set; }

    [Header("Knock back info")]
    [SerializeField] protected Vector2 knockBackDirection;

    [SerializeField] protected float knockBackDuration;
    private bool _isKnocked;

    [Header("Collision info")]
    public Transform attackCheck;

    public float attackCheckRadius;

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    private EntityFX _entityFX;

    public CharacterStats Stats { get; private set; }

    #region Components

    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public EntityFX FX { get; private set; }
    
    public CapsuleCollider2D CapsuleCollider { get; private set; }

    #endregion

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        FX = GetComponent<EntityFX>();
        Sr = GetComponentInChildren<SpriteRenderer>();
        Stats = GetComponent<CharacterStats>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {
    }

    public virtual void DamageEffect()
    {
        StartCoroutine(nameof(HitKnockBack));
        FX.Flash();
    }
    
    protected virtual IEnumerator HitKnockBack()
    {
        _isKnocked = true;

        Rb.linearVelocity = new Vector2(knockBackDirection.x * -FacingDir, knockBackDirection.y);

        yield return new WaitForSeconds(knockBackDuration);

        _isKnocked = false;
    }

    #region Collisions

    public virtual bool IsGroundDetected()
        => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsGroundDetected(float overrideDistance)
        => Physics2D.Raycast(groundCheck.position, Vector2.down, overrideDistance, whatIsGround);
    
    public virtual bool IsWallDetected()
        => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(groundCheck.position.x + wallCheckDistance * FacingDir, wallCheck.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    #region Flip

    protected virtual void FlipController(float xVelocity)
    {
        if(IsBusy) 
            return;
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

        onFlipped?.Invoke();
    }

    #endregion

    public void SetZeroVelocity()
    {
        if (_isKnocked) 
            return;
        Rb.linearVelocity = new Vector2(0, 0);
    }


    /// <summary>
    /// Called by PlayerMoveState to make movement for Player
    /// </summary>
    /// <remarks><see cref="Entity.FacingDir"/> can only be change through this message.</remarks>
    /// <param name="xVelocity">Velocity for x axis</param>
    /// <param name="yVelocity">Velocity for y axis</param>
    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    
    public IEnumerator BusyFor(float seconds)
    {
        IsBusy = true;

        yield return new WaitForSeconds(seconds);

        IsBusy = false;
    }

    public void SetTransparent(bool transparent)
    {
        Sr.color = transparent ? Color.clear : Color.white;
    }

    public virtual void Die()
    {
        
    }
}