using UnityEngine;

namespace Skills
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager Instance { get; private set; }

        public DashSkill Dash { get; private set; }
        public CloneSkill Clone { get; private set; }
        public SwordSkill Sword { get; private set; }
        public BlackHoleSkill BlackHole { get; private set; }
        public CrystalSkill Crystal { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            if (this == null) return;

            Dash = GetComponent<DashSkill>();
            Clone = GetComponent<CloneSkill>();
            Sword = GetComponent<SwordSkill>();
            BlackHole = GetComponent<BlackHoleSkill>();
            Crystal = GetComponent<CrystalSkill>();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
