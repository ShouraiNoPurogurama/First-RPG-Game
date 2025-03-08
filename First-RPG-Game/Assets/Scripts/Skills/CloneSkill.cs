using System.Collections;
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

        [Header("Clone special abilities")]
        [SerializeField] private bool createCloneOnDashStart;
        [SerializeField] private bool createCloneOnDashOver;
        [SerializeField] private bool canCreateCloneOnCounterAttack;
        [SerializeField] private bool canDuplicateClone;
        [SerializeField] private float chanceToDuplicate;

        [Header("Crystal instead of clone")]
        [SerializeField] public bool crystalInsteadOfClone;

        public void CreateClone(Transform clonePosition, Vector3 offset)
        {
            if (crystalInsteadOfClone)
            {
                SkillManager.Instance.Crystal.CreateCrystal();
                SkillManager.Instance.Crystal.CurrentCrystalChooseRandomTarget();
                return;
            }
            
            GameObject newClone = Instantiate(clonePrefab);

            Vector3 spawnPosition = clonePosition.position + offset;

            //Optimized with adding offset instead of using just original clone position 
            Transform closestEnemy = FindClosestEnemy(spawnPosition);

            newClone.GetComponent<CloneSkillController>()
                .SetupClone(clonePosition, cloneDuration, canAttack, offset, closestEnemy, canDuplicateClone, chanceToDuplicate, Player.FacingDir, Player);
        }

        public void CreateCloneOfDashStart()
        {
            if (createCloneOnDashStart)
            {
                CreateClone(Player.transform, Vector3.zero);
            }
        }

        public void CreateCloneOnDashOver()
        {
            if (createCloneOnDashOver)
            {
                CreateClone(Player.transform, Vector3.zero);
            }
        }

        public void CreateCloneOnCounterAttack(Transform enemyTransform)
        {
            if (canCreateCloneOnCounterAttack)
            {
                StartCoroutine(CreateCloneWithDelay(enemyTransform, new Vector3(1.5f*Player.FacingDir, 0)));
            }
        }

        private IEnumerator CreateCloneWithDelay(Transform spawnPosition, Vector3 offset)
        {
            yield return new WaitForSeconds(0.55f);
            CreateClone(spawnPosition, offset);
        }
    }
}