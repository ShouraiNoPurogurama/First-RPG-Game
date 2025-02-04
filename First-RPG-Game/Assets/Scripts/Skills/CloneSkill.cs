using Skills.SkillControllers;
using UnityEngine;

namespace Skills
{
    public class CloneSkill : Skill
    {
        [Header("Clone info")]
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private float cloneDuration;
        [Space]
        [SerializeField] private bool canAttack;

        public void CreateClone(Transform clonePosition)
        {
            GameObject newClone = Instantiate(clonePrefab);
            
            newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack);
        }
    }
}
