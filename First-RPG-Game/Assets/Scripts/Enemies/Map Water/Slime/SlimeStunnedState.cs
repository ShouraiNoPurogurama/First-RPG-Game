using Enemies;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    private Enemy_Slime enemy;
    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.FX.InvokeRepeating("RedColorBlink", 0, .1f);

        StateTimer = enemy.stunDuration;

        Rb.linearVelocity = new Vector2(-enemy.FacingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.Stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (Rb.linearVelocity.y < .1f && enemy.IsGroundDetected())
        {
            enemy.FX.Invoke("CancelColorChange", 0);
            enemy.Animator.SetTrigger("StunFold");
            enemy.Stats.MakeInvincible(true);
        }


        if (StateTimer < 0)
            StateMachine.ChangeState(enemy.idleState);
    }
}

