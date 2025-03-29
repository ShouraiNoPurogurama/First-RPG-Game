using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroDeadState : EnemyState
{
    private Hero _Hero;
    public HeroDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hero Hero) : base(enemyBase, stateMachine, animBoolName)
    {
        _Hero = Hero;
    }

    public override void Enter()
    {
        base.Enter();

        _Hero.Animator.SetBool(_Hero.LastAnimBoolName, true);
        _Hero.Animator.speed = 0;
        _Hero.CapsuleCollider.enabled = false;
        _Hero.transform.position = new Vector3(_Hero.transform.position.x, _Hero.transform.position.y, 10);

        StateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer > 0)
        {
            Rb.linearVelocity = new Vector2(0, 10);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
