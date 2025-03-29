using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroGroundedState : EnemyState
{
    protected Hero Hero;
    private Transform _player;

    protected HeroGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hero hero) : base(enemyBase, stateMachine, animBoolName)
    {
        Hero = hero;
    }
    public override void Enter()
    {
        base.Enter();

        _player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if (Hero.IsPlayerDetected() || Vector2.Distance(Hero.transform.position, _player.position) < 2)
        {
            StateMachine.ChangeState(Hero.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
