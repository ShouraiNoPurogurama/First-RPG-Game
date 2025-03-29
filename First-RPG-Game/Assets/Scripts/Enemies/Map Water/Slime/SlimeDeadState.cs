using Enemies;
using UnityEngine;

public class SlimeDeadState : EnemyState
{
    private Enemy_Slime enemy;
    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();

        enemy.Animator.SetBool(enemy.LastAnimBoolName, true);
        enemy.Animator.speed = 0;
        enemy.CapsuleCollider.enabled = false;

        StateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer > 0)
            Rb.linearVelocity = new Vector2(0, 10);
    }
}
