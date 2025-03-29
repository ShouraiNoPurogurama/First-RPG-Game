using Enemies;
using Enemies.Orc;
using MainCharacter;
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
namespace Enemies.Orc
{
    public class OrcBattleState : EnemyState
    {
        private Orc Orc;
        private Transform Player;

        private int moveDir;
        public OrcBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Orc orc) : base(enemyBase, stateMachine, animBoolName)
        {
            this.Orc = orc;
        }

        public override void Enter()
        {
            base.Enter();
            AttachCurrentPlayerIfNotExists();
        }

        public override void Update()
        {
            base.Update();
            if (Orc.IsWallDetected() || !Orc.IsGroundDetected())
            {
                Orc.Flip();
                return;
            }
            if (Orc.IsPlayerDetected())
            {
                StateTimer = Orc.battleTime;

                if (Orc.IsGroundDetected() &&
                    Orc.IsPlayerDetected().distance <= Orc.attackDistance &&
                    CanAttack())
                {
                    Debug.Log(Orc.IsPlayerDetected().collider.gameObject.name);
                    StateMachine.ChangeState(Orc.attackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(Player.transform.position, Orc.transform.position) > 7)
                {
                    StateMachine.ChangeState(Orc.idleState);
                }
            }
            moveDir = Player.position.x > Orc.transform.position.x ? 1 : -1;
            if (PlayerInAttackRange())
            {
                Orc.SetZeroVelocity();
            }

            Orc.SetVelocity(Orc.moveSpeed * moveDir, Rb.linearVelocity.y);
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = Orc.IsPlayerDetected().distance != 0 &&
                         Orc.IsPlayerDetected().distance <= Orc.attackDistance &&
                         (Orc.FacingDir == -1 && Player.transform.position.x <= Orc.transform.position.x ||
                          Orc.FacingDir == 1 && Player.transform.position.x >= Orc.transform.position.x);

            if (Mathf.Abs(Player.transform.position.x - Orc.transform.position.x) < Orc.attackDistance &&
                Mathf.Abs(Player.transform.position.y - Orc.transform.position.y) <=
                Orc.CapsuleCollider.bounds.size.y)
            {
                result = true;
            }

            return result;
        }

        private void AttachCurrentPlayerIfNotExists()
        {
            Player = GameObject.Find("Player").transform;
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(Orc.lastTimeAttacked, 0) ||
                Time.time >= Orc.lastTimeAttacked + Orc.attackCooldown)
            {
                return true;
            }

            return false;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
