using MainCharacter;
using UnityEngine;

namespace Skills.SkillControllers.Thunder_Map
{
    public class ThunderMapSkillManager : MonoBehaviour
    {
        [SerializeField] private GameObject swordPrefab; // Prefab của kiếm (cho skill ném nhiều kiếm)
        [SerializeField] private Player player; // Tham chiếu đến Player
        [SerializeField] private int swordCount = 1; // Số lượng kiếm ban đầu
        [SerializeField] private float spreadAngle = 20f; // Góc trải rộng khi ném nhiều kiếm
        [SerializeField] private float throwSpeed = 10f; // Tốc độ ném

        public Player Player => player; // Getter để TeleportSwordSkill truy cập

        private TeleportSwordSkill.TeleportSwordSkill _teleportSkill;

        private void Start()
        {
            _teleportSkill = GetComponent<TeleportSwordSkill.TeleportSwordSkill>(); // Lấy TeleportSwordSkill
            if (_teleportSkill != null)
            {
                _teleportSkill.EnableSkill(true); // Bật skill trong map Thunder
            }

            if (player == null)
            {
                Debug.LogError("Player not assigned in ThunderMapSkillManager Inspector.");
            }
        }

        private void OnDestroy()
        {
            if (_teleportSkill != null)
            {
                _teleportSkill.EnableSkill(false); // Tắt skill khi rời map
            }
        }

        void Update()
        {
            // Nhấn C để chuyển số lượng kiếm (1 -> 2 -> 3 -> quay lại 1)
            if (Input.GetKeyDown(KeyCode.C))
            {
                swordCount = (swordCount % 3) + 1; // Chuyển đổi: 1 -> 2 -> 3 -> 1
                Debug.Log($"Số lượng kiếm hiện tại: {swordCount}");
            }

            // Nhấn V để ném nhiều kiếm
            if (Input.GetKeyDown(KeyCode.V))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mousePos - (Vector2)player.transform.position).normalized;

                ThrowMultiSword(direction);
            }
        }

        private void ThrowMultiSword(Vector2 baseDirection)
        {
            float angleStep = swordCount > 1 ? spreadAngle / (swordCount - 1) : 0;
            float totalAngle = (swordCount - 1) * angleStep;
            float startAngle = -totalAngle / 2;

            for (int i = 0; i < swordCount; i++)
            {
                float angle = startAngle + i * angleStep;
                Vector2 adjustedDir = Quaternion.Euler(0, 0, angle) * baseDirection * throwSpeed;

                GameObject sword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
                SwordSkillController swordController = sword.GetComponent<SwordSkillController>();
                swordController.SetupSword(adjustedDir, 0, player, 0.5f, 15);
            }
        }
    }
}