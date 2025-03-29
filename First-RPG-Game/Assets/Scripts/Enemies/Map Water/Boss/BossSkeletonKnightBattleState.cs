using Enemies;
using MainCharacter;
using UnityEngine;

public class BossSkeletonKnightBattleState : EnemyState
{
    private BossSkeletonKnight enemy;
    private Transform _player;
    private int _moveDir;

    public BossSkeletonKnightBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, BossSkeletonKnight enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AttachCurrentPlayerIfNotExists();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected())
        {
            StateTimer = enemy.battleTime;

            if (enemy.IsGroundDetected() && enemy.IsPlayerDetected().distance <= enemy.attackDistance &&
                CanAttack())
            {
                StateMachine.ChangeState(enemy.AttackState);
                return;
            }
        }
        else
        {
            if (StateTimer < 0 || Vector2.Distance(_player.transform.position, enemy.transform.position) > 7)
            {
                StateMachine.ChangeState(enemy.IdleState);
            }
        }

        _moveDir = _player.position.x > enemy.transform.position.x ? 1 : -1;

        //if player in attack range, block skeleton movement
        if (PlayerInAttackRange())
        {
            enemy.SetZeroVelocity();
            return;
        }

        enemy.SetVelocity(enemy.moveSpeed * _moveDir, Rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public bool CanAttack()
    {
        AttachCurrentPlayerIfNotExists();

        if (Mathf.Approximately(enemy.lastTimeAttacked, 0) ||
            Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            return true;
        }

        return false;
    }

    public bool PlayerInAttackRange()
    {
        AttachCurrentPlayerIfNotExists();

        var result = enemy.IsPlayerDetected().distance <= enemy.attackDistance &&
               (enemy.FacingDir == -1 && _player.transform.position.x <= enemy.transform.position.x ||
                enemy.FacingDir == 1 && _player.transform.position.x >= enemy.transform.position.x);

        return result;
    }

    private void AttachCurrentPlayerIfNotExists()
    {
        if (!_player)
        {
            _player = PlayerManager.Instance.player.transform;
        }
    }
}
