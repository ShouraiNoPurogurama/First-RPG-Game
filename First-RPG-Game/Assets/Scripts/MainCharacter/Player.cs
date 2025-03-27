using MainCharacter.Water_Map;
using Skills;
using System;
using System.Linq;
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
            new(3f, 2f),
            new(1f, 3f),
            new(4f, 5f)
        };

        public float counterAttackDuration = .2f;
        public bool isDashAttack;

        private float _attackSpeed = 1;
        private float _defaultAttackSpeed;

        #endregion

        #region Player move

        [Header("Move info")]
        public float moveSpeed = 6;

        public float jumpForce = 12;

        public float swordReturnImpact = 8;

        private float _defaultMoveSpeed;

        private float _defaultJumpForce;

        #endregion

        #region Player Dash

        [Header("Dash info")]
        public float dashSpeed = 24;

        public float dashDuration = .1f;
        public float DashDir { get; private set; }

        private float _defaultDashSpeed;

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
        public PlayerCounterWaterAttack CounterWaterAttackState { get; private set; }
        public PlayerAimSwordState AimSwordState { get; private set; }
        public PlayerCatchSwordState CatchSwordState { get; private set; }
        public PlayerDashAttackState DashAttackState { get; private set; }
        public PlayerBlackHoleState BlackHoleState { get; private set; }
        public PlayerFallAfterAttackState FallAfterAttackState { get; private set; }
        public PlayerLandingAttackState LandingAttackState { get; private set; }
        public PlayerDeadState DeadState { get; private set; }

        #endregion


        public SkillManager SkillManager { get; private set; }
        public GameObject ThrownSword { get; private set; }
        public bool EquippedFinalAilment { get; set; }

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
            PrimaryAttackState = new PlayerPrimaryAttackState(StateMachine, this, "Attack", _attackSpeed);
            CounterAttackState = new PlayerCounterAttackState(StateMachine, this, "CounterAttack");
            CounterWaterAttackState = new PlayerCounterWaterAttack(StateMachine, this, "CounterAttack");
            AimSwordState = new PlayerAimSwordState(StateMachine, this, "AimSword");
            CatchSwordState = new PlayerCatchSwordState(StateMachine, this, "CatchSword");
            DashAttackState = new PlayerDashAttackState(StateMachine, this, "DashAttack");
            BlackHoleState = new PlayerBlackHoleState(StateMachine, this, "Jump");
            FallAfterAttackState = new PlayerFallAfterAttackState(StateMachine, this, "FallAfterAttack");
            LandingAttackState = new PlayerLandingAttackState(StateMachine, this, "LandingAttack");
            DeadState = new PlayerDeadState(StateMachine, this, "Die");
        }

        protected override void Start()
        {
            base.Start();

            StateMachine.Initialize(IdleState);

            SkillManager = SkillManager.Instance;

            _defaultMoveSpeed = moveSpeed;
            _defaultJumpForce = jumpForce;
            _defaultDashSpeed = dashSpeed;
            _defaultAttackSpeed = _attackSpeed;
        }

        protected override void Update()
        {
            base.Update();

            StateMachine.CurrentState.Update();

            CheckForDashInput();

            if (Input.GetKeyDown(KeyCode.F))
            {
                SkillManager.Crystal.CanUseSkill();
            }
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

        public void CheckForDashInput()
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
                    DashDir = FacingDir;
                }

                if (StateMachine.CurrentState == WallSlideState)
                {
                    DashDir = -FacingDir;
                }

                StateMachine.ChangeState(DashState);
            }
        }

        public override void ReduceAttackSpeedBy(float slowPercentage, float slowDuration)
        {
            //Increase attack cooldown bc we dont have attack speed
            _attackSpeed *= 1 - slowPercentage;

            Invoke("ReturnDefaultAttackSpeed", slowDuration);
        }

        protected override void ReturnDefaultAttackSpeed()
        {
            base.ReturnDefaultAttackSpeed();

            _attackSpeed = _defaultAttackSpeed;
        }

        public override void SlowEntityBy(float slowPercentage, float slowDuration)
        {
            moveSpeed *= 1 - slowPercentage;
            jumpForce *= 1 - slowPercentage;
            dashSpeed *= 1 - slowPercentage;
            Animator.speed *= 1 - slowPercentage;

            Invoke("ReturnDefaultSpeed", slowDuration);
        }

        public override void FastEntityBy(float increasePercentage, float increaseDuration)
        {
            moveSpeed *= 1 + increasePercentage;
            jumpForce *= 1 + increasePercentage;
            dashSpeed *= 1 + increasePercentage;
            Animator.speed *= 1 + increasePercentage;

            Invoke("ReturnDefaultSpeed", increaseDuration);
        }


        protected override void ReturnDefaultSpeed()
        {
            base.ReturnDefaultSpeed();

            moveSpeed = _defaultMoveSpeed;
            jumpForce = _defaultJumpForce;
            dashSpeed = _defaultDashSpeed;
        }

        public override void Die()
        {
            base.Die();

            StateMachine.ChangeState(DeadState);
        }

        public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

        // knock player
        public void BossAttackPlayerKnock(Vector2 vt)
        {
            knockBackPower = vt;
        }    
    }
}