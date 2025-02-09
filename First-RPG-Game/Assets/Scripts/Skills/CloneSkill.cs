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

        public void CreateClone(Transform clonePosition, Vector3 offset)
        {
            GameObject newClone = Instantiate(clonePrefab);

            Vector3 spawnPosition = clonePosition.position + offset;
            
            //Optimized with adding offset instead of using just original clone position 
            Transform closestEnemy = FindClosestEnemy(spawnPosition);
            
            newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, offset, closestEnemy);
        }
    }
}
