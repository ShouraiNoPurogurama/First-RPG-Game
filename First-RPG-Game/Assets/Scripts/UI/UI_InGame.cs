using Skills;
using Stats;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{

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

    [Header("Màu sắc UI")]
    [SerializeField] private Color color1 = Color.red;
    [SerializeField] private Color color2 = Color.blue;
    [SerializeField] private Color color3 = Color.green;
    [SerializeField] private Color color4 = Color.yellow;
    [SerializeField] private Color color5 = Color.magenta;

    private SkillManager skillManager;
    private CharacterStats characterStats;
    private PlayerStats playerStats;

    void Start()
    {
        skillManager = SkillManager.Instance;

        playerStats = FindObjectOfType<PlayerStats>();

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

        // Cập nhật màu UI theo cooldown
        UpdateUIColor();
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

    private void UpdateUIColor()
    {
    
        int fireDamageVal = playerStats.fireDamage.ModifiedValue;
        int iceDamageVal = playerStats.iceDamage.ModifiedValue;
        int lightningDamageVal = playerStats.lightingDamage.ModifiedValue;
        //Debug.Log(iceDamageVal);

     
        Color selectedColor;
        if (fireDamageVal >= iceDamageVal && fireDamageVal >= lightningDamageVal)
            selectedColor = color1; 
        else if (iceDamageVal >= fireDamageVal && iceDamageVal >= lightningDamageVal)
            selectedColor = color2; 
        else
            selectedColor = color3; 


        dashSkillImage.color = selectedColor;
        crystalSkillImage.color = selectedColor;
        cloneSkillImage.color = selectedColor;
        blackholeSkillImage.color = selectedColor;
        swordSkillImage.color = selectedColor;
    }
}
