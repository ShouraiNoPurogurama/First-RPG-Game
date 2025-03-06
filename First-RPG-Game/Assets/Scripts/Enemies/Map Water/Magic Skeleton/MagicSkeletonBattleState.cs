using Enemies;
using MainCharacter;
using UnityEngine;
public class MagicSkeletonBattleState : EnemyState
{
    private Enemy_Magic_Skeleton magicSkeleton;
    private Transform _player;
    private int _moveDir;

    public MagicSkeletonBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        magicSkeleton = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // _player = GameObject.Find("Player").transform;
        AttachCurrentPlayerIfNotExists();

        //if player in attack range, block skeleton movement
        if (PlayerInAttackRange() && !CanAttack())
        {
            magicSkeleton.SetZeroVelocity();
            StateMachine.ChangeState(magicSkeleton.IdleState);
        }
    }

    public override void Update()
    {
        base.Update();

        if (magicSkeleton.IsPlayerDetected())
        {
            StateTimer = magicSkeleton.battleTime;

            if (magicSkeleton.IsPlayerDetected().distance < magicSkeleton.safeDistance)
            {
                if (CanJump())
                {
                    StateMachine.ChangeState(magicSkeleton.JumpState);
                    return;
                }
            }

            if (magicSkeleton.IsGroundDetected() && magicSkeleton.IsPlayerDetected().distance <= magicSkeleton.attackDistance &&
                CanAttack())
            {
                StateMachine.ChangeState(magicSkeleton.AttackState);
                return;
            }
        }
        else
        {
            if (StateTimer < 0 || Vector2.Distance(_player.transform.position, magicSkeleton.transform.position) > 7)
            {
                StateMachine.ChangeState(magicSkeleton.IdleState);
            }
        }

        _moveDir = _player.position.x > magicSkeleton.transform.position.x ? 1 : -1;

        //if player in attack range, block skeleton movement
        if (PlayerInAttackRange())
        {
            magicSkeleton.SetZeroVelocity();
            StateMachine.ChangeState(magicSkeleton.IdleState);
            return;
        }

        magicSkeleton.SetVelocity(magicSkeleton.moveSpeed * _moveDir, Rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public bool CanAttack()
    {
        AttachCurrentPlayerIfNotExists();

        if (Mathf.Approximately(magicSkeleton.lastTimeAttacked, 0) || Time.time >= magicSkeleton.lastTimeAttacked + magicSkeleton.attackCooldown)
        {
            magicSkeleton.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    public bool PlayerInAttackRange()
    {
        AttachCurrentPlayerIfNotExists();

        var result = magicSkeleton.IsPlayerDetected().distance <= magicSkeleton.attackDistance &&
               (magicSkeleton.FacingDir == -1 && _player.transform.position.x <= magicSkeleton.transform.position.x ||
                magicSkeleton.FacingDir == 1 && _player.transform.position.x >= magicSkeleton.transform.position.x);

        return result;
    }

    private void AttachCurrentPlayerIfNotExists()
    {
        if (_player == null)
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.player != null)
            {
                _player = PlayerManager.Instance.player.transform;
            }
            else
            {
                Debug.LogWarning("Player not found, attempting to find by tag...");
                GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
                if (foundPlayer != null)
                {
                    _player = foundPlayer.transform;
                }
            }
        }
    }

    private bool CanJump()
    {
        if (Time.time >= magicSkeleton.lastTimeJumped + magicSkeleton.JumpCooldown)
        {
            magicSkeleton.lastTimeJumped = Time.time;
            return true;
        }
        return false;
    }
}
