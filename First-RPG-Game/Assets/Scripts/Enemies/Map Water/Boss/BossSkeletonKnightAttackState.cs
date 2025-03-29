using Enemies;
using UnityEngine;
public class BossSkeletonKnightAttackState : EnemyState
{
    private BossSkeletonKnight enemy;
    private int _comboCounter;
    private readonly float _comboWindow = 2f;

    public BossSkeletonKnightAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, BossSkeletonKnight enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (Time.time >= enemy.lastTimeAttacked + _comboWindow)
        {
            _comboCounter = 0;
        }
        enemy.Animator.SetInteger("ComboCounter", _comboCounter);
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (TriggerCalled)
        {
            Debug.Log("Attack trigger called");
            TriggerCalled = false;
            enemy.lastTimeAttacked = Time.time;
            if (enemy.CanTeleport())
                StateMachine.ChangeState(enemy.DisappearState);
            else
                StateMachine.ChangeState(enemy.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _comboCounter++;
        if (_comboCounter > 2)
        {
            _comboCounter = 0;
        }
    }
}
