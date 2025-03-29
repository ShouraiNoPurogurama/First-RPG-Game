using MainCharacter;
using UnityEngine;

namespace Skills.SkillControllers.Thunder_Map.TeleportSwordSkill
{
    public class TeleportSwordSkill : MonoBehaviour
    {
        [SerializeField] private GameObject teleportSwordPrefab; // Prefab kiếm dịch chuyển
        [SerializeField] private float swordSpeed = 15f; // Tốc độ bay của kiếm
        [SerializeField] private float swordGravity = 0f; // Không chịu trọng lực (bay thẳng)
        [SerializeField] private bool isEnabled = true; // Bật/tắt skill

        private Player _player;
        private ThunderMapSkillManager _skillManager;

        private void Start()
        {
            _skillManager = GetComponent<ThunderMapSkillManager>(); // Lấy ThunderMapSkillManager
            if (_skillManager != null)
            {
                _player = _skillManager.Player; // Lấy Player từ ThunderMapSkillManager
            }

            if (_player == null)
            {
                Debug.LogError("Player not found in ThunderMapSkillManager. Please assign the Player in the Inspector.");
            }
        }

        private void Update()
        {
            if (!isEnabled || _player == null) return; // Không chạy nếu skill bị tắt hoặc Player không được gán

            if (Input.GetKeyDown(KeyCode.X))
            {
                ThrowTeleportSword();
            }
        }

        public void ThrowTeleportSword()
        {
            Vector2 direction = new Vector2(_player.FacingDir, 0).normalized;
            GameObject newSword = Instantiate(teleportSwordPrefab, _player.transform.position, Quaternion.identity);
            TeleportSwordController controller = newSword.GetComponent<TeleportSwordController>();

            controller.Setup(direction * swordSpeed, swordGravity, _player);
        }

        public void EnableSkill(bool enable)
        {
            isEnabled = enable;
        }
    }
}