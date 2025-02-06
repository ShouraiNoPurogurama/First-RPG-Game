using System;
using System.Collections;
using System.Linq;
using Skills;
using UnityEngine;

namespace MainCharacter
{
    public class Player : Entity
    {
        
        #region Consts

        private readonly Type[] _canDashStates = 
        {
            typeof(PlayerIdleState),
            typeof(PlayerMoveState),
            typeof(PlayerJumpState),
            typeof(PlayerAirState),
            typeof(PlayerWallSlideState),
        };

        #endregion

        #region Player attack

        [Header("Attack details")]
        public Vector2[] attackMovements =
        {
            new (3f, 2f),
            new (1f, 3f),
            new (4f, 5f)
        };

        public float counterAttackDuration = .2f;
        public bool isDashAttack;

        #endregion

        #region Player move

        [Header("Move info")]
        public float moveSpeed = 6;

        public float jumpForce = 12;

        public float swordReturnImpact = 8;

        #endregion

        #region Player Dash

        [Header("Dash info")]
        public float dashSpeed = 24;
        public float dashDuration = .1f;
        public float DashDir { get; private set; }

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
        public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
        public PlayerCounterAttackState CounterAttackState { get; private set; }
        public PlayerAimSwordState AimSwordState { get; private set; }
        public PlayerCatchSwordState CatchSwordState { get; private set; }
        public PlayerDashAttackState DashAttackState { get; private set; }
        public PlayerBlackHoleState BlackHoleState { get; private set; }

        #endregion

        // #region Skill Cooldown
        //
        // public float Timer;
        // public float Cooldown;
        //
        // #endregion

        public SkillManager SkillManager { get; private set; } 
        public GameObject ThrownSword { get; private set; } 
        

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
            WallJumpState = new PlayerWallJumpState(StateMachine, this, "Jump");
            DashState = new PlayerDashState(StateMachine, this, "Dash");
            WallSlideState = new PlayerWallSlideState(StateMachine, this, "WallSlide");

            PrimaryAttackState = new PlayerPrimaryAttackState(StateMachine, this, "Attack");
            CounterAttackState = new PlayerCounterAttackState(StateMachine, this, "CounterAttack");
            AimSwordState = new PlayerAimSwordState(StateMachine, this, "AimSword");
            CatchSwordState = new PlayerCatchSwordState(StateMachine, this, "CatchSword");
            DashAttackState = new PlayerDashAttackState(StateMachine, this, "DashAttack");
            BlackHoleState = new PlayerBlackHoleState(StateMachine, this, "Jump");
        }

        protected override void Start()
        {
            base.Start();

            StateMachine.Initialize(IdleState);

            SkillManager = SkillManager.Instance;
        }

        protected override void Update()
        {
            base.Update();

            StateMachine.CurrentState.Update();
            CheckForDashInput();
        }

        public void AssignNewSword(GameObject newSword)
        {
            ThrownSword = newSword;
        }

        public void CatchSword()
        {
            StateMachine.ChangeState(CatchSwordState);
            Destroy(ThrownSword);
        }

        public void CheckForDashInput(float? forcedDirection = null)
        {
            if (!_canDashStates.Contains(StateMachine.CurrentState.GetType()))
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.Dash.CanUseSkill())
            {
                DashDir = Input.GetAxisRaw("Horizontal");

                if (DashDir == 0)
                {
                    DashDir = forcedDirection ?? FacingDir;
                }

                StateMachine.ChangeState(DashState);
            }
        }

        public void ExitBlackHoleAbility()
        {
            StateMachine.ChangeState(AirState);
        }
        
        public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    }
}