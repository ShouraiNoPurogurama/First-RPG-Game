using Enemies;
using Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Hero : Enemy
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    public HeroAttackState AttackState { get; set; }
    public HeroBattleState BattleState { get; set; }
    public HeroDeadState DeadState { get; set; }
    public HeroGroundedState GroundedState { get; set; }
    public HeroMoveState MoveState { get; set; }
    public HeroIdleState IdleState { get; set; }
    public HeroStunnedState StunnedState { get; set; }


    protected override void Awake()
    {
        base.Awake();
        IdleState = new HeroIdleState(this, StateMachine, "Idle", this);
        MoveState = new HeroMoveState(this, StateMachine, "Move", this);
        BattleState = new HeroBattleState(this, StateMachine, "Move", this);
        AttackState = new HeroAttackState(this, StateMachine, "Attack", this);
        DeadState = new HeroDeadState(this, StateMachine, "Dead", this);
        StunnedState = new HeroStunnedState(this, StateMachine, "Stunned", this);
        counterImage.SetActive(false);

    }
    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    public override bool IsCanBeStunned(bool forceStun)
    {
        if (base.IsCanBeStunned(forceStun))
        {
            StateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    public override void Flip()
    {
        if (IsBusy)
            return;

        base.Flip();

        StartCoroutine("BusyFor", .3f);
    }

    public override void Die()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
        StateMachine.ChangeState(DeadState);
        base.Die();
    }

    public void ShootArrows()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null)
        {
            Debug.LogWarning("Arrow Prefab or SpawnPoint is missing!");
            return;
        }

        Vector3 spawnPos = arrowSpawnPoint.position;
        Quaternion arrowRotationQuat = Quaternion.identity;

        GameObject arrow = Instantiate(arrowPrefab, spawnPos, arrowRotationQuat);
        Arrow_Controller arrowController = arrow.GetComponent<Arrow_Controller>();

        if (arrowController != null)
        {
            arrowController.SetVelocity(Vector2.right * 3 * transform.localScale.x);
        }
        else
        {
            Debug.LogWarning("Arrow_Controller is missing on the arrow prefab!");
        }
    }
}
