using TMPro;
using UnityEngine;

namespace Skills.SkillControllers
{
    public class BlackHoleHotkeyController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private KeyCode _myHotKey;
        private TextMeshProUGUI _myText;

        private Transform _enemy;
        private BlackHoleSkillController _blackHole;

        public void SetupHotKey(KeyCode myNewHotKey, Transform enemy, BlackHoleSkillController myBlackHole)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _enemy = enemy;
            _blackHole = myBlackHole;
            
            _myText = GetComponentInChildren<TextMeshProUGUI>();
            _myHotKey = myNewHotKey;
            _myText.text = myNewHotKey.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_myHotKey) && !_blackHole.ContainsEnemy(_enemy))
            {
                _blackHole.AddEnemyToList(_enemy);

                _myText.color = Color.clear;
                _spriteRenderer.color = Color.clear;
            }
        }
    }
}