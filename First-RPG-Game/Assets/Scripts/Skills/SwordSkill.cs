using UnityEngine;

namespace Skills
{
    public class SwordSkill : Skill
    {
        [Header("Skill info")]
        [SerializeField] private GameObject swordPrefab;
        [SerializeField] private Vector2 launchDir;
        [SerializeField] private float swordGravity;

        public void CreateSword()
        {
            GameObject newSword = Instantiate(swordPrefab, Player.transform.position, transform.rotation);

            SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();
            newSwordScript.SetupSword(launchDir, swordGravity);

        }
    }
}
