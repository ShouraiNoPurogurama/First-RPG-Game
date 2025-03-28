using Stats;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HPBar_UI : MonoBehaviour
    {
        private Entity _entity;
        private CharacterStats _characterStats;
        private RectTransform _rectTransform;
        private Slider _slider;
        private Image _fillImage; 

        [SerializeField] private Color normalColor = Color.red;
        [SerializeField] private Color lowHPColor = Color.yellow; 

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _entity = GetComponentInParent<Entity>();
            _slider = GetComponentInChildren<Slider>();
            
            _characterStats = GetComponentInParent<CharacterStats>();

            // Lấy component Image từ Fill Area
            _fillImage = _slider.fillRect.GetComponent<Image>();

            _characterStats.OnHPChanged += UpdateHealthUI;
            _entity.OnFlipped += FlipUI;

            UpdateHealthUI();
        }

        private void UpdateHealthUI()
        {
            float maxHp = _characterStats.GetMaxHealthValue();
            float currentHp = _characterStats.currentHp;

            _slider.maxValue = maxHp;
            _slider.value = currentHp;

            // change if hp < 30%
            if (currentHp / maxHp <= 0.3f)
            {
                _fillImage.color = lowHPColor;
            }
            else
            {
                _fillImage.color = normalColor;
            }
        }

        private void FlipUI()
        {
            _rectTransform.Rotate(0, 180, 0);
        }

        private void OnDisable()
        {
            _entity.OnFlipped -= FlipUI;
            _characterStats.OnHPChanged -= UpdateHealthUI;
        }
    }
}
