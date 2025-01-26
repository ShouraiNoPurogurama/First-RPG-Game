using System;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Consts

    private readonly Type[] canDashStates = new []
    {
        typeof(PlayerIdleState),
        typeof(PlayerMoveState),
        typeof(PlayerJumpState),
        typeof(PlayerAirState),
        typeof(PlayerWallSlideState),
    };
    #endregion
    
    [Header("Move info")]
    public float moveSpeed = 6;

    public float jumpForce = 12;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown = 2;

    private float _dashUsageTimer;
    public float dashSpeed = 24;
    public float dashDuration = .1f;
    public float DashDir { get; private set; }

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    public int FacingDir { get; private set; } = 1;
    private bool isFacingRight = true;

    #region Components

    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    #endregion

    #region States

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }

    public PlayerPrimaryAttack PrimaryAttack { get; set; }

    #endregion

    #region SkillCooldown

    public float Timer;
    public float Cooldown;

    #endregion

    /// <summary>
    /// Initialize player states when first awoke
    /// <example>Flow: Player => Initialize PlayerStateMachine => Enter PlayerState</example>
    /// </summary>
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(StateMachine, this, "Idle");
        MoveState = new PlayerMoveState(StateMachine, this, "Move");
        JumpState = new PlayerJumpState(StateMachine, this, "Jump");
        AirState = new PlayerAirState(StateMachine, this, "Jump");
        DashState = new PlayerDashState(StateMachine, this, "Dash");
        WallSlideState = new PlayerWallSlideState(StateMachine, this, "WallSlide");
        WallJumpState = new PlayerWallJumpState(StateMachine, this, "Jump");
        
        PrimaryAttack = new PlayerPrimaryAttack(StateMachine, this, "Attack");
    }

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
        CheckForDashInput();
    }

    /// <summary>
    /// Called by PlayerMoveState to make movement for Player
    /// </summary>
    /// <remarks><see cref="FacingDir"/> can only be change through this message.</remarks>
    /// <param name="xVelocity">Velocity for x axis</param>
    /// <param name="yVelocity">Velocity for y axis</param>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.linearVelocity = new Vector2(xVelocity * moveSpeed, yVelocity);
        FlipController(xVelocity);
    }

    public bool IsGroundDetected()
        => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public bool IsWallDetected()
        => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(groundCheck.position.x + wallCheckDistance * FacingDir, groundCheck.position.y));
    }

    public void FlipController(float xVelocity)
    {
        if (xVelocity > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (xVelocity < 0 && isFacingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void CheckForDashInput(float? forcedDirection = null)
    {
        if (!canDashStates.Contains(StateMachine.CurrentState.GetType()))
        {
            return;
        }

        _dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashUsageTimer <= 0)
        {
            _dashUsageTimer = dashCooldown;
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir == 0)
            {
                DashDir = forcedDirection ?? FacingDir;
            }
            StateMachine.ChangeState(DashState);
        }
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
}