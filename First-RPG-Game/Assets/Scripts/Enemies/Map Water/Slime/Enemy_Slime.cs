using Enemies;
using UnityEngine;

public enum SlimeType { big, medium, small }

public class Enemy_Slime : Enemy
{
    [Header("Slime spesific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimesToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

    #region States

    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefailtFacingDir(-1);

        idleState = new SlimeIdleState(this, StateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, StateMachine, "Move", this);
        battleState = new SlimeBattleState(this, StateMachine, "Move", this);
        attackState = new SlimeAttackState(this, StateMachine, "Attack", this);

        stunnedState = new SlimeStunnedState(this, StateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, StateMachine, "Idle", this);

    }


    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        //if (Input.GetKeyDown(KeyCode.D))
        //    CreateSlimes(slimesToCreate, slimePrefab);
    }

    public override bool IsCanBeStunned(bool stun)
    {
        if (base.IsCanBeStunned(false))
        {
            StateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }



    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(deadState);

        if (slimeType == SlimeType.small)
            return;

        CreateSlimes(slimesToCreate, slimePrefab);

    }

    private void CreateSlimes(int _amountOfSlimes, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlimes; i++)
        {
            Vector2 spawnOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.2f, 0.5f));
            Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
            GameObject newSlime = Instantiate(_slimePrefab, spawnPosition, Quaternion.identity);
            newSlime.GetComponent<Enemy_Slime>().SetupSlime(FacingDir);
        }
    }

    public void SetupSlime(int _facingDir)
    {

        if (_facingDir != FacingDir)
            Flip();

        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x + 10f);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        _isKnocked = true;

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity * -FacingDir, yVelocity);

        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback() => _isKnocked = false;
}
