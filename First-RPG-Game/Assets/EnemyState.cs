using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine StateMachine;
    protected Enemy EnemyBase;
    protected Rigidbody2D Rb;

    protected bool TriggerCalled;
    protected string AnimBoolName;
    protected float StateTimer;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        StateMachine = stateMachine;
        EnemyBase = enemyBase;
        AnimBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        TriggerCalled = false;
        Rb = EnemyBase.Rb;
        EnemyBase.Animator.SetBool(AnimBoolName, true);
    }

    public virtual void Update()
    {
        StateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        EnemyBase.Animator.SetBool(AnimBoolName, false);
    }
}