using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity _entity;
    private CharacterStats _characterStats;
    private RectTransform _rectTransform;
    private Slider _slider;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _entity = GetComponentInParent<Entity>();
        _slider = GetComponentInChildren<Slider>();
        _characterStats = GetComponentInParent<CharacterStats>();
        _characterStats.OnHealthChanged += UpdateHealthUI;
        
        _entity.OnFlipped += FlipUI;
        
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        _slider.maxValue = _characterStats.GetMaxHealthValue();
        _slider.value = _characterStats.currentHp;
    }
    
    private void FlipUI()
    {
        _rectTransform.Rotate(0, 180, 0);
    }
    
    private void OnDisable()
    {
        _entity.OnFlipped -= FlipUI;
        _characterStats.OnHealthChanged -= UpdateHealthUI;
    }
}