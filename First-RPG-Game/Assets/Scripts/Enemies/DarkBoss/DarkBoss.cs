using Skills;
using UnityEngine;

namespace Enemies.DarkBoss
{
    public class DarkBoss : Enemy
    {
        public DarkBossIdleState IdleState { get; private set; }
        public DarkBossMoveState MoveState { get; private set; }
        public DarkBossBattleState BattleState { get; private set; }
        public DarkBossDeadState DeadState { get; private set; }
        public DarkBossAttackState AttackState { get; private set; }
        public DarkBossTeleportState TeleportState { get; private set; }
        public DarkBossCastState CastState { get; private set; }
        public DarkBossSummonState SummonState { get; private set; }

        [Header("Teleport info")]
        [SerializeField] public BoxCollider2D arena;
        [SerializeField] private Vector2 surroundingCheckSize;
        [SerializeField] public float teleportCooldown;
        [HideInInspector]
        public float lastTimeTeleport;
        public float moveCooldown;

        [Header("Cast animation")]
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private int arrowCount = 16; 
        [SerializeField] private float spawnRadius = 2f; 
        [SerializeField] private float arrowSpeed = 5f;

        [Header("Summon")]
        [SerializeField] private GameObject summonPrefab;
        [SerializeField] private int summonCount = 3;

        public bool IsCallSummoned { get; set; } = true;
        protected override void Awake()
        {
            base.Awake();
            IdleState = new DarkBossIdleState(this, StateMachine, "Idle", this);
            MoveState = new DarkBossMoveState(this, StateMachine, "Move", this);
            BattleState = new DarkBossBattleState(this, StateMachine, "Move", this);
            DeadState = new DarkBossDeadState(this, StateMachine, "Idle", this);
            AttackState = new DarkBossAttackState(this, StateMachine, "Attack", this);
            TeleportState = new DarkBossTeleportState(this, StateMachine, "Teleport", this);
            CastState = new DarkBossCastState(this, StateMachine, "Cast", this);
            SummonState = new DarkBossSummonState(this, StateMachine, "Summon", this);
            counterImage.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();
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
        public void FindPosition()
        {
            //float x = UnityEngine.Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
            //float y = UnityEngine.Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);
            //transform.position = new Vector3(x, y);
            ;
            transform.position = new Vector3(GameObject.Find("Player").transform.position.x - 1, GameObject.Find("Player").transform.position.y, 0);

            if (!GroundBelow()  || SomethingIsArround())
            {
                FindPosition();
            }
        }
        private RaycastHit2D GroundBelow()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
        }
        private bool SomethingIsArround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
            Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y+3), surroundingCheckSize);
        }

        public bool CanTeleport()
        {
            return Time.time > lastTimeTeleport + teleportCooldown;
        }

        public void ShootArrows()
        {
            float angleStep = 360f / arrowCount; 
            float angle = 0f; 

            for (int i = 0; i < arrowCount; i++)
            {
                float radian = angle * Mathf.Deg2Rad;
                Vector2 spawnPos = new Vector2(
                    transform.position.x + spawnRadius * Mathf.Cos(radian),
                    transform.position.y + spawnRadius * Mathf.Sin(radian)
                );

                Vector2 shootDirection = (spawnPos - (Vector2)transform.position).normalized;

                float arrowRotation = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                Quaternion arrowRotationQuat = Quaternion.Euler(0, 0, arrowRotation);

                GameObject arrow = Instantiate(arrowPrefab, spawnPos, arrowRotationQuat);
                Arrow_Controller arrowController = arrow.GetComponent<Arrow_Controller>();

                arrowController.SetVelocity(shootDirection * arrowSpeed);

                angle += angleStep; 
            }
        }

        public void Summon()
        {
            for (int i = 0; i < summonCount; i++)
            {
                Vector2 spawnPos = new Vector2(
                    GameObject.Find("Player").transform.position.x + UnityEngine.Random.Range(-10, 10),
                    GameObject.Find("Player").transform.position.y
                );
                Instantiate(summonPrefab, spawnPos, Quaternion.identity);
            }
        }


    }
}
