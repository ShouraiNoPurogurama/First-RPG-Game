using Audio;
using Skills.SkillControllers;
using UnityEngine;

namespace Skills
{
    public class BlackHoleSkill : Skill
    {
        [SerializeField] private float maxSize;
        [SerializeField] private float growSpeed;
        [SerializeField] private float shrinkSpeed;
        [SerializeField] private float blackHoleDuration;
        [SerializeField] private int totalTriggerAttackAmount;

        [SerializeField] private GameObject blackHolePrefab;
        [SerializeField] private int attackAmount;
        [SerializeField] private float cloneAttackCooldown;

        private BlackHoleSkillController _currentBlackHole;

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
            SoundManager.PlaySFX("Attack", 5);

            GameObject newBlackHole = Instantiate(blackHolePrefab, Player.transform.position, Quaternion.identity);

            _currentBlackHole = newBlackHole.GetComponent<BlackHoleSkillController>();

            _currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, attackAmount, cloneAttackCooldown, blackHoleDuration, totalTriggerAttackAmount);
        }

        public bool SkillFinished()
        {
            if (!_currentBlackHole) 
                return false;

            if (_currentBlackHole.PlayerCanExitState)
            {
                _currentBlackHole = null;
                SoundManager.StopSFX();
                return true;
            }

            return false;
        }

        public float GetBlackHoleRadius() => maxSize / 2; //Actual radius of black hole
    }
}