using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine StateMachine;
    protected Enemy Enemy;

    protected bool TriggerCalled;
    private string _animBoolName;
    private float _stateTimer;

    public EnemyState(Enemy enemy, bool triggerCalled, string animBoolName)
    {
        Enemy = enemy;
        this.TriggerCalled = triggerCalled;
        this._animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        TriggerCalled = false;
        Enemy.Animator.SetBool(_animBoolName, true);
    }

    public virtual void Update()
    {
        _stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        Enemy.Animator.SetBool(_animBoolName, false);
    }
}