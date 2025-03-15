using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class GiantAttackState : EnemyState
{
    protected Giant Giant;
    public GiantAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Giant giant) : base(enemyBase, stateMachine, animBoolName)
    {
        Giant = giant;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        Giant.SetZeroVelocity();

        if (TriggerCalled)
        {
            TriggerCalled = false;
            Giant.lastTimeAttacked = Time.time;
            StateMachine.ChangeState(Giant.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
