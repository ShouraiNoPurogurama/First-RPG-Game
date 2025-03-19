using Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Soul
{
    public class EnemySoul : Enemy
    {
        [Header("Soul specific info")]
        public float battleStateMoveSpeed;
        [SerializeField] private GameObject explosivePrefab;
        [SerializeField] private GameObject magicEnergyPrefab;
        [SerializeField] private float magicEnergySpeed;
        [SerializeField] private float growSpeed;
        [SerializeField] private float maxSize;

        [FormerlySerializedAs("TriggerLeapDistance")] public float explodeDistance; //How close player should be to trigger jump on battle state
        private CharacterStats _myStats;

        #region States

        public SoulIdleState IdleState { get; private set; }
        public SoulMoveState MoveState { get; private set; }
        public SoulBattleState BattleState { get; private set; }
        public SoulAttackState AttackState { get; private set; }
        public SoulDeadState DeadState { get; private set; }
        public SoulStunnedState StunnedState { get; private set; }

        #endregion

        
        //TODO
        //If hp >= 30%, be invincible and flash + raise an attack 
        //If hp < 30%, move quickly to player and explode
        
        protected override void Awake()
        {
            base.Awake();

            IdleState = new SoulIdleState(this, StateMachine, "Idle", this);
            MoveState = new SoulMoveState(this, StateMachine, "EnterMove", this);
            BattleState = new SoulBattleState(this, StateMachine, "MoveFast", this);
            AttackState = new SoulAttackState(this, StateMachine, "Shot", this);
            DeadState = new SoulDeadState(this, StateMachine, "Dead", this);
            StunnedState = new SoulStunnedState(this, StateMachine, "Stunned", this);
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

        public override void AnimationSpecialAttackTrigger()
        {
            GameObject newExplosive = Instantiate(explosivePrefab, transform.position, Quaternion.identity);
            
            newExplosive.GetComponent<ExplosiveController>().SetupExplosive(Stats, growSpeed, maxSize, attackCheckRadius);
            CapsuleCollider.enabled = false;
            Rb.gravityScale = 0;
        }

        public override void SecondaryAnimationSpecialAttackTrigger()
        {
            GameObject newMagicEnergy = Instantiate(magicEnergyPrefab, attackCheck.position, Quaternion.identity);

            newMagicEnergy.GetComponent<MagicEnergyController>().SetupMagicEnergy(magicEnergySpeed * FacingDir, Stats);
        }

        public void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}