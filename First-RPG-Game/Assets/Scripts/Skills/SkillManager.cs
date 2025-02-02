using UnityEngine;

namespace Skills
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager Instance { get; private set; }

        public DashSkill Dash { get; private set; }
        public CloneSkill Clone { get; private set; }
        public SwordSkill Sword { get; private set; }

        private void Awake()
        {
            if (Instance is not null && Instance != this)
            {
                Destroy(Instance.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            Dash = GetComponent<DashSkill>();
            Clone = GetComponent<CloneSkill>();
            Sword = GetComponent<SwordSkill>();
        }
    }
}