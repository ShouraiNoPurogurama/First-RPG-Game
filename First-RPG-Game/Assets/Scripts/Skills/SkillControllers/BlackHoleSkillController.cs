using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skills.SkillControllers
{
    public class BlackHoleSkillController : MonoBehaviour
    {
        [SerializeField] private GameObject hotkeyPrefab;
        [SerializeField] private List<KeyCode> keyCodes;
        
        public float maxSize;
        public float growSpeed;
        public bool canGrow;

        [SerializeField] private List<Transform> targets = new();

        private void Update()
        {
            if (canGrow)
            {
                transform.localScale =
                    Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy is not null)
            {
                enemy.FreezeTime(true);

                CreateHotkey(collision, enemy);

                // newHotKey.GetComponent<BlackHoleHotkeyController>().SetupHotKey();
                // targets.Add(collision.transform);
                //Respawn prefab of a hot key above enemy
            }
        }

        private void CreateHotkey(Collider2D collision, Enemy enemy)
        {
            if (keyCodes.Count <= 0)
            {
                Debug.Log("Not enough hot keys in the key code list.");
                return;
            }
            
            GameObject newHotKey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

            KeyCode chosenKey = keyCodes[Random.Range(0, keyCodes.Count)];
            
            keyCodes.Remove(chosenKey);

            BlackHoleHotkeyController newHotkeyScript = newHotKey.GetComponent<BlackHoleHotkeyController>();
                
            newHotkeyScript.SetupHotKey(chosenKey, enemy.transform, this); //Attach each enemy to each hot key 
        }

        public void AddEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform);

        public bool ContainsEnemy(Transform enemyTransform) => targets.Contains(enemyTransform);
    }
}