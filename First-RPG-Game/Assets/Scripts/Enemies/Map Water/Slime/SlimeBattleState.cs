using Enemies;
using MainCharacter;
using Stats;
using UnityEngine;

public class SlimeBattleState : EnemyState
{
    private Enemy_Slime enemy;
    private Transform _player;
    private int _moveDir;

    public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();

        _player = PlayerManager.Instance.player.transform;

        if (_player.GetComponent<PlayerStats>().isDead)
            StateMachine.ChangeState(enemy.moveState);


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
                StateMachine.ChangeState(enemy.attackState);
                return;
            }
        }
        else
        {
            if (StateTimer < 0 || Vector2.Distance(_player.transform.position, enemy.transform.position) > 7)
            {
                StateMachine.ChangeState(enemy.idleState);
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
    private void AttachCurrentPlayerIfNotExists()
    {
        if (!_player)
        {
            _player = PlayerManager.Instance.player.transform;
        }
    }
    public bool PlayerInAttackRange()
    {
        AttachCurrentPlayerIfNotExists();

        var result = enemy.IsPlayerDetected().distance <= enemy.attackDistance &&
               (enemy.FacingDir == -1 && _player.transform.position.x <= enemy.transform.position.x ||
                enemy.FacingDir == 1 && _player.transform.position.x >= enemy.transform.position.x);

        return result;
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
