using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    #region Consts

    private readonly Type[] canDashStates = new[]
    {
        typeof(PlayerIdleState),
        typeof(PlayerMoveState),
        typeof(PlayerJumpState),
        typeof(PlayerAirState),
        typeof(PlayerWallSlideState),
    };

    #endregion

    public Vector2[] attackMovements =
    {
        new Vector2(3f, 2f),
        new Vector2(1f, 3f),
        new Vector2(4f, 5f)
    };

    [Header("Move info")]
    public float moveSpeed = 6;

    public float jumpForce = 12;

    public bool IsBusy { get; private set; }
    
    [Header("Dash info")]
    [SerializeField] private float dashCooldown = 2;

    private float _dashUsageTimer;
    public float dashSpeed = 24;
    public float dashDuration = .1f;
    public float DashDir { get; private set; }


    #region States

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }

    public PlayerPrimaryAttackState PrimaryAttackState { get; set; }

    #endregion

    #region SkillCooldown

    public float Timer;
    public float Cooldown;

    #endregion

    /// <summary>
    /// Initialize player states when first awoke
    /// <example>Flow: Player => Initialize PlayerStateMachine => Enter PlayerState</example>
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(StateMachine, this, "Idle");
        MoveState = new PlayerMoveState(StateMachine, this, "Move");
        JumpState = new PlayerJumpState(StateMachine, this, "Jump");
        AirState = new PlayerAirState(StateMachine, this, "Jump");
        DashState = new PlayerDashState(StateMachine, this, "Dash");
        WallSlideState = new PlayerWallSlideState(StateMachine, this, "WallSlide");
        WallJumpState = new PlayerWallJumpState(StateMachine, this, "Jump");

        PrimaryAttackState = new PlayerPrimaryAttackState(StateMachine, this, "Attack");
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.Update();
        CheckForDashInput();
    }

    public IEnumerator BusyFor(float seconds)
    {
        IsBusy = true;

        yield return new WaitForSeconds(seconds);

        IsBusy = false;
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