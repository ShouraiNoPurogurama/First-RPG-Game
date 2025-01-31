using UnityEngine;

namespace MainCharacter
{
    public class PlayerPrimaryAttackState : PlayerState
    {
        private int _comboCounter;

        private float _lastTimeAttacked;
        private readonly float _comboWindow = 2;

        public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine,
            player, animationBoolName)
        {
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
            Player.Animator.speed = 1.2f;

            Player.SetVelocity(Player.attackMovements[_comboCounter].x * attackDir / Player.moveSpeed,
                Player.attackMovements[_comboCounter].y);

            StateTimer = .1f;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer < 0)
            {
                Player.SetZeroVelocity();
            }

            //Change state when player trigger is called (animation ends)
            if (TriggerCalled)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Player.StartCoroutine("BusyFor", .15f);
            Player.Animator.speed = 1;

            _comboCounter++;
            _lastTimeAttacked = Time.time;
        }
    }
}