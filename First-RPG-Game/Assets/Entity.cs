using UnityEngine;

public class Entity : MonoBehaviour
{
    public int FacingDir { get; protected set; } = 1;
    private bool _isFacingRight = true;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;

    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    #region Components

    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    #endregion

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
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
            new Vector3(groundCheck.position.x + wallCheckDistance * FacingDir, groundCheck.position.y));
    }

    #endregion

    #region Flip

    protected void FlipController(float xVelocity)
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

    private void Flip()
    {
        FacingDir *= -1;
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    #endregion
}