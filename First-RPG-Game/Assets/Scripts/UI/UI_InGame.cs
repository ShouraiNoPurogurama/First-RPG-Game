using Skills;
using Stats;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_InGame : MonoBehaviour
    {
        [Header("HP UI")]
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private Slider slider;
        [SerializeField] private Color normalColor = Color.red;
        [SerializeField] private Color lowHPColor = Color.yellow;

        [Header("Skill UI")]
        [SerializeField] private Image dashSkillImage;
        [SerializeField] private Image crystalSkillImage;
        [SerializeField] private Image cloneSkillImage;
        [SerializeField] private Image blackholeSkillImage;
        [SerializeField] private Image swordSkillImage;

        [Header("Cooldown UI")]
        [SerializeField] private Image dashImage;
        [SerializeField] private Image crystalImage;
        [SerializeField] private Image cloneImage;
        [SerializeField] private Image blackholeImage;
        [SerializeField] private Image swordImage;

        [Header("Color UI")]
        [SerializeField] private Color color1 = Color.red;
        [SerializeField] private Color color2 = Color.blue;
        [SerializeField] private Color color3 = Color.yellow;
        [SerializeField] private Color color4 = new Color(0.545f, 0.271f, 0.075f);
        [SerializeField] private Color color5 = Color.green;

        private SkillManager skillManager;
        private CharacterStats _characterStats;
        private Image _fillImage;

        void Start()
        {
            skillManager = SkillManager.Instance;

            if (playerStats == null)
            {
                Debug.LogError("UI_InGame: PlayerStats is missing!");
                enabled = false;
                return;
            }

            _characterStats = playerStats;
            _fillImage = slider?.fillRect?.GetComponent<Image>();

            if (slider == null || _fillImage == null)
            {
                Debug.LogError("UI_InGame: Slider or FillImage is missing!");
                enabled = false;
                return;
            }

            _characterStats.OnHPChanged += UpdateHealthUI;
            UpdateHealthUI();
        }

        void Update()
        {
            if (skillManager == null) return;

            if (Input.GetKeyDown(KeyCode.LeftShift))
                SetCooldownOf(dashImage);

            if (Input.GetKeyDown(KeyCode.R))
                SetCooldownOf(blackholeImage);

            if (Input.GetKeyDown(KeyCode.Mouse1))
                SetCooldownOf(swordImage);

            if (Input.GetKeyDown(KeyCode.Mouse0))
                SetCooldownOf(crystalImage);

            if (Input.GetKeyDown(KeyCode.F))
                SetCooldownOf(cloneImage);

            CheckCooldownOf(dashImage, skillManager.Dash.CoolDown);
            CheckCooldownOf(swordImage, skillManager.Sword.CoolDown);
            CheckCooldownOf(cloneImage, skillManager.Clone.CoolDown);
            CheckCooldownOf(crystalImage, skillManager.Crystal.CoolDown);
            CheckCooldownOf(blackholeImage, skillManager.BlackHole.CoolDown);

            UpdateUIColor();
        }

        private void SetCooldownOf(Image _image)
        {
            if (_image != null && _image.fillAmount <= 0)
                _image.fillAmount = 1;
        }

        private void CheckCooldownOf(Image _image, float _cooldown)
        {
            if (_image != null && _cooldown > 0 && _image.fillAmount > 0)
                _image.fillAmount -= Time.deltaTime / _cooldown;
        }

        private void UpdateUIColor()
        {
            if (playerStats == null) return;

            int[] damageValues = {
                playerStats.fireDamage.ModifiedValue,
                playerStats.iceDamage.ModifiedValue,
                playerStats.lightingDamage.ModifiedValue,
                playerStats.earthDamage.ModifiedValue,
                playerStats.windDamage.ModifiedValue
            };

            Color[] colors = { color1, color2, color3, color4, color5 };

            int maxIndex = 0;
            for (int i = 1; i < damageValues.Length; i++)
            {
                if (damageValues[i] > damageValues[maxIndex])
                    maxIndex = i;
            }

            Color selectedColor = colors[maxIndex];

            // Gán màu cho tất cả skill images
            dashSkillImage.color = selectedColor;
            crystalSkillImage.color = selectedColor;
            cloneSkillImage.color = selectedColor;
            blackholeSkillImage.color = selectedColor;
            swordSkillImage.color = selectedColor;
        }

        private void UpdateHealthUI()
        {
            if (_characterStats == null || slider == null || _fillImage == null)
            {
                Debug.LogError("UI_InGame: CharacterStats, Slider, or FillImage is missing!");
                return;
            }

            float maxHp = _characterStats.GetMaxHealthValue();
            float currentHp = _characterStats.currentHp;

            slider.maxValue = maxHp;
            slider.value = currentHp;

            _fillImage.color = (currentHp / maxHp <= 0.3f) ? lowHPColor : normalColor;
        }

        private void OnDestroy()
        {
            if (_characterStats != null)
                _characterStats.OnHPChanged -= UpdateHealthUI;
        }
    }
}
