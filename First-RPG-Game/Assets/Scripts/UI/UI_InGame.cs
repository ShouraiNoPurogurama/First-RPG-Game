using Skills;
using Stats;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [Header("Cooldown UI")]
    [SerializeField] private Image dashImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image cloneImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image flaskImage;

    [Header("Major stats UI")]
    [SerializeField] private Image statImage; 
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite iceSprite;
    [SerializeField] private Sprite lightningSprite;

    private SkillManager skillManager;
    private Inventory inventory;
    private CharacterStats characterStats;

    private int currentStatIndex = 0;

    void Start()
    {
        skillManager = SkillManager.Instance;
        inventory = Inventory.instance;
        characterStats = FindObjectOfType<PlayerStats>();

        statImage.sprite = fireSprite;
        SetStat(0);
    }

    // Update is called once per frame
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, skillManager.Dash.CoolDown);
        CheckCooldownOf(swordImage, skillManager.Sword.CoolDown);
        CheckCooldownOf(cloneImage, skillManager.Clone.CoolDown);
        CheckCooldownOf(crystalImage, skillManager.Crystal.CoolDown);
        CheckCooldownOf(blackholeImage, skillManager.BlackHole.CoolDown);

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentStatIndex = (currentStatIndex + 1) % 3; // loop
            SetStat(currentStatIndex);
        }
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if(_image.fillAmount >0)
            _image.fillAmount -= 1 / _cooldown *Time.deltaTime;
    }

    private void SetStat(int index)
    {
        if (characterStats == null)
        {
            Debug.LogError("characterStats is NULL in SetStat!");
            return;
        }

        if (characterStats.fireDamage == null || characterStats.iceDamage == null || characterStats.lightingDamage == null)
        {
            Debug.LogError("One of the damage stats is NULL! Check CharacterStats initialization.");
            return;
        }

        switch (index)
        {
            case 0: // Fire
                characterStats.fireDamage.SetDefaultValue(100);
                characterStats.iceDamage.SetDefaultValue(100);
                characterStats.lightingDamage.SetDefaultValue(100);
                statImage.sprite = fireSprite;
                break;
            case 1: // Ice
                characterStats.fireDamage.SetDefaultValue(0);
                characterStats.iceDamage.SetDefaultValue(100);
                characterStats.lightingDamage.SetDefaultValue(0);
                statImage.sprite = iceSprite;
                break;
            case 2: // Lightning
                characterStats.fireDamage.SetDefaultValue(0);
                characterStats.iceDamage.SetDefaultValue(0);
                characterStats.lightingDamage.SetDefaultValue(100);
                statImage.sprite = lightningSprite;
                break;
        }
    }
}
