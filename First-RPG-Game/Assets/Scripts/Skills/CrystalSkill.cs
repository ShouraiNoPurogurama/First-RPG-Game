using Skills.SkillControllers;
using UnityEngine;

namespace Skills
{
    public class CrystalSkill : Skill
    {
        [SerializeField] private GameObject crystalPrefab;
        [SerializeField] private float crystalDuration;

        private GameObject _currentCrystal;

        [Header("Moving crystal")]
        [SerializeField] private bool canMoveToEnemy;

        [SerializeField] private float moveSpeed;

        [Header("Explode crystal")]
        [SerializeField] private bool canExplode;

        public override void UseSkill()
        {
            base.UseSkill();

            if (!_currentCrystal)
            {
                _currentCrystal = Instantiate(crystalPrefab, Player.transform.position, Quaternion.identity);

                CrystalSkillController currentCrystalScript = _currentCrystal.GetComponent<CrystalSkillController>();

                currentCrystalScript.SetUpCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed);
            }
            else
            {
                Vector2 playerPosition = Player.transform.position;
                
                Player.transform.position = _currentCrystal.transform.position;

                _currentCrystal.transform.position = playerPosition;
                
                _currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
            }
        }
    }
}