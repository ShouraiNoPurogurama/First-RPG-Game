using UnityEngine;
using UnityEngine.UI;

public class HPBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private CharacterStats myStats;
    private Slider slider;


    void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI;

        myStats.onHPChanged += UpdateHpUI;

        UpdateHpUI();

    }

    private void Update()
    {
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
         entity.onFlipped -= FlipUI;

        myStats.onHPChanged -= UpdateHpUI;
    }

}
