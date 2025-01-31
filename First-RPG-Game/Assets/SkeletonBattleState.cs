using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private EnemySkeleton _skeleton;
    private Transform _player;
    private int _moveDir;
    
    public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) : base(enemyBase, stateMachine, animBoolName)
    {
        _skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log(("Im in battle state"));
        
        _player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if (_skeleton.IsPlayerDetected())
        {
            if (_skeleton.IsPlayerDetected().distance < _skeleton.attackDistance)
            {
                Debug.Log("I ATTACK");
                _skeleton.SetZeroVelocity();
                return;
            }
        }
        
        if (_player.position.x > _skeleton.transform.position.x)
        {
            _moveDir = 1;
        } else if (_player.position.x < _skeleton.transform.position.x)
        {
            _moveDir = -1;
        }
        
        _skeleton.SetVelocity(_skeleton.moveSpeed * _moveDir, Rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
