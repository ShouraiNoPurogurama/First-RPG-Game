using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroAttackState : EnemyState
{
    private Hero _Hero;
    public HeroAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hero Hero) : base(enemyBase, stateMachine, animBoolName)
    {
        _Hero = Hero;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        _Hero.SetZeroVelocity();

        if (TriggerCalled)
        {
            _Hero.ShootArrows();
            TriggerCalled = false;
            _Hero.lastTimeAttacked = Time.time;
            StateMachine.ChangeState(_Hero.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
