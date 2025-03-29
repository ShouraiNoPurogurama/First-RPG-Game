using Enemies;
using MainCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroBattleState : EnemyState
{
    private Hero _Hero;
    private Transform _player;
    private int _moveDir;
    public HeroBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hero Hero) : base(enemyBase, stateMachine, animBoolName)
    {
        _Hero = Hero;
    }
    public override void Enter()
    {
        base.Enter();
        AttachCurrentPlayerIfNotExists();
    }

    public override void Update()
    {
        base.Update();

        if (_Hero.IsPlayerDetected())
        {
            StateTimer = _Hero.battleTime;

            if (_Hero.IsGroundDetected() &&
                _Hero.IsPlayerDetected().distance <= _Hero.attackDistance &&
                CanAttack())
            {
                StateMachine.ChangeState(_Hero.AttackState);
                return;
            }
        }
        else
        {
            if (StateTimer < 0 || Vector2.Distance(_player.transform.position, _Hero.transform.position) > 7)
            {
                StateMachine.ChangeState(_Hero.IdleState);
            }
        }

        _moveDir = _player.position.x > _Hero.transform.position.x ? 1 : -1;

        //if player in attack range, block skeleton movement
        if (PlayerInAttackRange())
        {
            _Hero.SetZeroVelocity();
            //StateMachine.ChangeState(_Hero.IdleState);
            return;
        }

        _Hero.SetVelocity(_Hero.moveSpeed * _moveDir, Rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public bool CanAttack()
    {
        AttachCurrentPlayerIfNotExists();

        if (Mathf.Approximately(_Hero.lastTimeAttacked, 0) ||
            Time.time >= _Hero.lastTimeAttacked + _Hero.attackCooldown)
        {
            return true;
        }

        return false;
    }

    public bool PlayerInAttackRange()
    {
        AttachCurrentPlayerIfNotExists();

        var result = _Hero.IsPlayerDetected().distance != 0 &&
                     _Hero.IsPlayerDetected().distance <= _Hero.attackDistance &&
                     (_Hero.FacingDir == -1 && _player.transform.position.x <= _Hero.transform.position.x ||
                      _Hero.FacingDir == 1 && _player.transform.position.x >= _Hero.transform.position.x);

        if (Mathf.Abs(_player.transform.position.x - _Hero.transform.position.x) < _Hero.attackDistance &&
            Mathf.Abs(_player.transform.position.y - _Hero.transform.position.y) <=
            _Hero.CapsuleCollider.bounds.size.y)
        {
            result = true;
        }

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
