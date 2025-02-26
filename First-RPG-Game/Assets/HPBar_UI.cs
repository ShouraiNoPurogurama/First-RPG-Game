using UnityEngine;
using UnityEngine.UI;

public class HPBar_UI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform rectTransform;
    private Slider slider;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        if (entity == null)
        {
            Debug.LogError("Entity is NULL in HPBar_UI", this);
        }
        else
        {
            entity.onFlipped += FlipUI;
        }

        if (slider == null)
        {
            Debug.LogError("Slider is NULL in HPBar_UI", this);
        }

        if (myStats == null)
        {
            Debug.LogError("CharacterStats is NULL in HPBar_UI", this);
        }
        else
        {
            myStats.OnHealthChanged += UpdateHpUI; 
        }

        UpdateHpUI(); 
    }

    private void UpdateHpUI()
    {
        if (slider == null || myStats == null) return;

        slider.maxValue = myStats.maxHp.FinalValue;
        slider.value = myStats.currentHp;
    }

    private void FlipUI() => rectTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
            myStats.OnHealthChanged -= UpdateHpUI;
    }
}
