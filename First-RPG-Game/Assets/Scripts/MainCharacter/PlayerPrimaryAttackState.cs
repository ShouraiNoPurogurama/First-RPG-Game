using UnityEngine;

namespace MainCharacter
{
    public class PlayerPrimaryAttackState : PlayerState
    {
        private int _comboCounter;

        private float _lastTimeAttacked;
        private readonly float _comboWindow = 2;
        private float _attackSpeed;

        public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player player, string animationBoolName, float attackSpeed = 1) : base(stateMachine,
            player, animationBoolName)
        {
            _attackSpeed = attackSpeed;
        }

        public override void Enter()
        {
            base.Enter();


            xInput = 0; //Fix bug on attack direction

            if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
            {
                _comboCounter = 0;
            }

            float attackDir = Player.FacingDir;

            if (xInput != 0)
            {
                attackDir = xInput;
            }

            Player.Animator.SetInteger("ComboCounter", _comboCounter);
            Player.Animator.speed = _attackSpeed;

            float xAttackVelocity = Player.isDashAttack
                ? Player.moveSpeed * attackDir
                : Player.attackMovements[_comboCounter].x * attackDir;
            
            Player.SetVelocity(xAttackVelocity, Player.attackMovements[_comboCounter].y);

            StateTimer = .1f;

            Player.isDashAttack = false;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer < 0 && Player.IsGroundDetected() && !Player.isDashAttack)
            {
                Player.SetZeroVelocity();
            }
            else if(!Player.IsGroundDetected() && Player.isDashAttack)
            {
                Player.SetVelocity(Rb.linearVelocity.x, 0);
            }

            if (TriggerCalled && !Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.AirState);
            }
            
            //Change state when player trigger is called (animation ends)
            else if (TriggerCalled)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Player.StartCoroutine("BusyFor", .1f);
            Player.Animator.speed = 1;

            _comboCounter++;
            _lastTimeAttacked = Time.time;
        }
    }
}