using System.Collections.Generic;
using System.Threading;
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

        [Header("Multi stacking crystal")]
        [SerializeField] private bool canUseMultiStacks;

        [SerializeField] private int stackAmount;
        [SerializeField] private float multiStackCooldown;
        [SerializeField] private float useTimeWindow; //If last time use skill exceeds this duration, reset the skill
        [SerializeField] private List<GameObject> crystalLeft = new();

        [Header("Crystal mirrage")]
        [SerializeField] private bool cloneInsteadOfCrystal;

        public override void UseSkill()
        {
            base.UseSkill();

            if (CanUseMultiCrystal())
            {
                return;
            }

            if (!_currentCrystal)
            {
                CreateCrystal();
            }
            else
            {
                if (canMoveToEnemy)
                {
                    return; //Restrict the player from swap position with moving crystal
                }

                Vector2 playerPosition = Player.transform.position;
                Player.transform.position = _currentCrystal.transform.position;
                _currentCrystal.transform.position = playerPosition;

                if (cloneInsteadOfCrystal)
                {
                    SkillManager.Instance.Clone.CreateClone(_currentCrystal.transform, Vector3.zero);
                    Destroy(_currentCrystal);
                }
                else
                {
                    _currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
                }
            }
        }

        public void CreateCrystal()
        {
            _currentCrystal = Instantiate(crystalPrefab, Player.transform.position, Quaternion.identity);

            CrystalSkillController currentCrystalScript = _currentCrystal.GetComponent<CrystalSkillController>();

            currentCrystalScript.SetUpCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
                FindClosestEnemy(_currentCrystal.transform.position), Player);
        }

        public void CurrentCrystalChooseRandomTarget() =>
            _currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();

        private void RefillCrystal()
        {
            int amountToAdd = stackAmount - crystalLeft.Count;

            for (int i = 0; i < amountToAdd; i++)
            {
                crystalLeft.Add(crystalPrefab);
            }
        }

        private bool CanUseMultiCrystal()
        {
            if (canUseMultiStacks)
            {
                if (crystalLeft.Count > 0)
                {
                    if (crystalLeft.Count == stackAmount)
                    {
                        Invoke("ResetAbility", useTimeWindow);
                    }

                    CooldownTimer = -1; //Allows continuous use of the crystal as long as there are remaining crystals.
                    GameObject crystalToSpawn = crystalLeft[^1]; //Index from end of collection
                    GameObject newCrystal = Instantiate(crystalToSpawn, Player.transform.position, Quaternion.identity);

                    crystalLeft.Remove(crystalToSpawn);

                    newCrystal.GetComponent<CrystalSkillController>()
                        .SetUpCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
                            FindClosestEnemy(newCrystal.transform.position), Player);

                    if (crystalLeft.Count <= 0)
                    {
                        CoolDown = multiStackCooldown;
                        RefillCrystal();
                    }
                }

                return true;
            }

            return false;
        }

        private void ResetAbility()
        {
            if (CooldownTimer > 0) //Assure the skill is not on cooldown already
                return;

            CooldownTimer = multiStackCooldown; //Wait for multiStackCooldown for next usage
            RefillCrystal();
        }
    }
}