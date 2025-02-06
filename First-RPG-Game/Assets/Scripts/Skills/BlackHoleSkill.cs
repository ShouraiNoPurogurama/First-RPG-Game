using Skills.SkillControllers;
using UnityEngine;

namespace Skills
{
    public class BlackHoleSkill : Skill
    {
        [SerializeField] private float maxSize;
        [SerializeField] private float growSpeed;
        [SerializeField] private float shrinkSpeed;
        
        [SerializeField] private GameObject blackHolePrefab;
        [SerializeField] private int attackAmount;
        [SerializeField] private float cloneAttackCooldown;
        
        
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override bool CanUseSkill()
        {
            return base.CanUseSkill();
        }

        public override void UseSkill()
        {
            base.UseSkill();

            GameObject newBlackHole = Instantiate(blackHolePrefab, Player.transform.position, Quaternion.identity);

            BlackHoleSkillController newBlackHoleScript = newBlackHole.GetComponent<BlackHoleSkillController>();
            
            newBlackHoleScript.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, attackAmount, cloneAttackCooldown);
        }
    }
}

