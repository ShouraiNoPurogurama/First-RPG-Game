using Enemies;
using MainCharacter;
using Unity.Mathematics;
using UnityEngine;

namespace Skills
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] public float CoolDown;
        protected float CooldownTimer;
        protected Player Player;

        protected virtual void Start()
        {
            Player = PlayerManager.Instance.player;
        }

        protected virtual void Update()
        {
            CooldownTimer -= Time.deltaTime;
        }

        public bool CanUseSkillWithNoLogic()
        {
            if (CooldownTimer <= 0)
            {
                return true;
            }

            Debug.Log("Creating popup text");
            Player.FX.CreatePopUpText("Cooldown!", Color.white);
            return false;
        }
        
        public virtual bool CanUseSkill()
        {
            if (CooldownTimer <= 0)
            {
                UseSkill();
                CooldownTimer = CoolDown;
                Debug.Log("Creating popup text");
                return true;
            }

            Player.FX.CreatePopUpText("CoolDown", null);
            
        
            return false;
        }

        public virtual void UseSkill()
        {
            //Do some skill specific things
        }

        protected virtual Transform FindClosestEnemy(Vector3 position)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 25);

            float closestDistance = math.INFINITY;
            Transform closestEnemy = null;

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() is not null)
                {
                    float distanceToEnemy = Vector2.Distance(position, hit.transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = hit.transform;
                    }
                }
            }
            return closestEnemy;
        }
    }
}
