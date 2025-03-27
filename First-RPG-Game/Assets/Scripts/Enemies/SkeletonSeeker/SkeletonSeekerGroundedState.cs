using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkeletonSeekerGroundedState : EnemyState
{
    protected SkeletonSeeker SkeletonSeeker;
    private Transform _player;

    protected SkeletonSeekerGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName)
    {
        SkeletonSeeker = skeletonSeeker;
    }
    public override void Enter()
    {
        base.Enter();

        _player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if (SkeletonSeeker.IsPlayerDetected() || Vector2.Distance(SkeletonSeeker.transform.position, _player.position) < 2)
        {
            StateMachine.ChangeState(SkeletonSeeker.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
