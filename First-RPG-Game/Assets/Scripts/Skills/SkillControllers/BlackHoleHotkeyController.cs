using System.Collections;
using TMPro;
using UnityEngine;

namespace Skills.SkillControllers
{
    public class BlackHoleHotkeyController : MonoBehaviour
    {
        private KeyCode _myHotKey;
        public TextMeshProUGUI MyText { get; private set; }

        private BlackHoleSkillController _blackHole;
        
        public Transform Enemy { get; private set; }

        public void SetupHotKey(KeyCode myNewHotKey, Transform enemy, BlackHoleSkillController myBlackHole)
        {
            Enemy = enemy;
            _blackHole = myBlackHole;
            
            MyText = GetComponentInChildren<TextMeshProUGUI>();
            _myHotKey = myNewHotKey;
            MyText.text = myNewHotKey.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_myHotKey) && !_blackHole.ContainsEnemy(Enemy))
            {
                _blackHole.AddEnemyToList(Enemy);
            }
        }

        
        public IEnumerator ChangeColorTemporarily(float seconds)
        {
            MyText.color = new Color(235 / 255f, 91 / 255f, 52 / 255f);
            yield return new WaitForSeconds(seconds);
            MyText.color = Color.white;
        }

    }
}