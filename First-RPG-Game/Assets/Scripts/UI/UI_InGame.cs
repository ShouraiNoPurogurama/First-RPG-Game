using Skills;
using Stats;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
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

        [Header("Color UI")]
        [SerializeField] private Color color1 = Color.red;
        [SerializeField] private Color color2 = Color.blue;
        [SerializeField] private Color color3 = Color.yellow;
        [SerializeField] private Color color4 = new Color(0.545f, 0.271f, 0.075f);
        [SerializeField] private Color color5 = Color.green;

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
            int earthDamageVal = playerStats.earthDamage.ModifiedValue;
            int windDamageVal = playerStats.windDamage.ModifiedValue;



            Color selectedColor;

            if (fireDamageVal >= iceDamageVal && fireDamageVal >= lightningDamageVal &&
                fireDamageVal >= earthDamageVal && fireDamageVal >= windDamageVal)
            {
                selectedColor = color1; 
            }
            else if (iceDamageVal >= fireDamageVal && iceDamageVal >= lightningDamageVal &&
                     iceDamageVal >= earthDamageVal && iceDamageVal >= windDamageVal)
            {
                selectedColor = color2; 
            }
            else if (lightningDamageVal >= fireDamageVal && lightningDamageVal >= iceDamageVal &&
                     lightningDamageVal >= earthDamageVal && lightningDamageVal >= windDamageVal)
            {
                selectedColor = color3; 
            }
            else if (earthDamageVal >= fireDamageVal && earthDamageVal >= iceDamageVal &&
                     earthDamageVal >= lightningDamageVal && earthDamageVal >= windDamageVal)
            {
                selectedColor = color4; 
            }
            else
            {
                selectedColor = color5; 
            }


            dashSkillImage.color = selectedColor;
            crystalSkillImage.color = selectedColor;
            cloneSkillImage.color = selectedColor;
            blackholeSkillImage.color = selectedColor;
            swordSkillImage.color = selectedColor;
        }
    }
}
