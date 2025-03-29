using Stats;
using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int FacingDir { get; private set; } = 1;
    private bool _isFacingRight = true;

    public bool IsBusy { get; private set; }

    public SpriteRenderer Sr { get; private set; }

    [Header("Knock back info")]
    [SerializeField] protected Vector2 knockBackPower = new Vector2(5, 5);

    [SerializeField] protected float knockBackDuration = 0.25f;
    public bool _isKnocked;

    [Header("Collision info")]
    public Transform attackCheck;

    public float attackCheckRadius;

    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundCheckDistance = 0.6f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] public Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 0.54f;
    private EntityFX _entityFX;

    public Action OnFlipped;

    public int KnockBackDir { get; private set; }

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

    public virtual void ReduceAttackSpeedBy(float slowPercentage, float slowDuration)
    {

    }

    public virtual void SlowEntityBy(float slowPercentage, float slowDuration)
    {
    }

    public virtual void FastEntityBy(float slowPercentage, float slowDuration)
    {
    }

    protected virtual void ReturnDefaultAttackSpeed()
    {
    }

    protected virtual void ReturnDefaultSpeed()
    {
        Animator.speed = 1;
    }

    public virtual void DamageImpact()
    {
        StartCoroutine(nameof(HitKnockBack));
    }

    public virtual void SetupKnockBackDir(Transform damageDirection)
    {
        if (damageDirection.position.x >= transform.position.x)
        {
            KnockBackDir = -1;
        }
        else if (damageDirection.position.x < transform.position.x)
        {
            KnockBackDir = 1;
        }
    }

    protected virtual IEnumerator HitKnockBack()
    {
        _isKnocked = true;
        Rb.linearVelocity = new Vector2(knockBackPower.x * KnockBackDir, knockBackPower.y);

        yield return new WaitForSeconds(knockBackDuration);
        _isKnocked = false;

        Rb.linearVelocity = new Vector2(KnockBackDir, Rb.linearVelocity.y);
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
        if (IsBusy)
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

        if (OnFlipped != null)
        {
            OnFlipped();
        }
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
    public virtual void SetupDefailtFacingDir(int _direction)
    {
        FacingDir = _direction;

        if (FacingDir == -1)
            _isFacingRight = false;
    }
}