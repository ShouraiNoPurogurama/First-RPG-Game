using System.Collections.Generic;
using Cinemachine;
using Enemies;
using MainCharacter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skills.SkillControllers
{
    public class BlackHoleSkillController : MonoBehaviour
    {
        [SerializeField] private GameObject hotkeyPrefab;
        [SerializeField] private List<KeyCode> keyCodes;

        [Header("Black hole info")]
        private float _maxSize;
        private float _growSpeed;
        private float _shrinkSpeed;
        private bool _canGrow = true;
        private bool _canShrink;

        [Header("Clone attack info")]
        private int _attackAmount = 4;
        private float _cloneAttackCooldown = .3f;
        private float _cloneAttackTimer;
        private bool _canCreateHotKeys = true;
        private bool _cloneAttackReleased;

        [Header("Camera")]
        private GameObject _virtualCamera;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private Transform _originalCameraTarget;

        [SerializeField] private List<Transform> targets = new();
        [SerializeField] private List<GameObject> createdHotKeys = new();

        public void SetupBlackHole(float maxSize, float growSpeed, float shrinkSpeed, int attackAmount, float cloneAttackCooldown)
        {
            _maxSize = maxSize;
            _growSpeed = growSpeed;
            _shrinkSpeed = shrinkSpeed;
            _attackAmount = attackAmount;
            _cloneAttackCooldown = cloneAttackCooldown;
            
            _virtualCamera = GameObject.Find("Virtual Camera");
            _cinemachineVirtualCamera = _virtualCamera.GetComponent<CinemachineVirtualCamera>();
            _originalCameraTarget = _cinemachineVirtualCamera.m_Follow;
        }

        private void Update()
        {
            _cloneAttackTimer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R))
            {
                ReleaseCloneAttack();
            }

            CloneAttackLogic();

            if (_canGrow && !_canShrink)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);
                
                //Stop growing when max size is reached
                if (Mathf.Abs(transform.localScale.x - _maxSize) < 0.01f) 
                {
                    transform.localScale = new Vector2(_maxSize, _maxSize);
                    _canGrow = false;
                }
            }

            if (_canShrink)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed * Time.deltaTime);
                if (transform.localScale.x < 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void ReleaseCloneAttack()
        {
            if (targets.Count == 0) 
                return;
            
            DestroyHotKeys();
            _cloneAttackReleased = true;
            _canCreateHotKeys = false;
            
            PlayerManager.Instance.player.SetTransparent(true);
        }

        private void CloneAttackLogic()
        {
            if (_cloneAttackTimer <= 0 && _cloneAttackReleased)
            {
                _cloneAttackTimer = _cloneAttackCooldown;
                
                int randomIndex = Random.Range(0, targets.Count - 1);

                if (randomIndex < 0) randomIndex = 0;

                float xOffset = Random.Range(0, 100) > 50 ? 1.5f : -1.5f;

                SkillManager.Instance.Clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));

                _cinemachineVirtualCamera.Follow = targets[randomIndex].transform;
                    
                _attackAmount--;

                if (_attackAmount <= 0)
                {
                    Invoke("FinishBlackHoleAbility", 1f);
                }
            }
        }

        private void FinishBlackHoleAbility()
        {
            _canShrink = true;
            _cloneAttackReleased = false;
            _cinemachineVirtualCamera.Follow = _originalCameraTarget;
            PlayerManager.Instance.player.ExitBlackHoleAbility(); //Exit black hole state when it over
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

        private void DestroyHotKeys()
        {
            if (createdHotKeys.Count <= 0) return;

            foreach (var hotKey in createdHotKeys)
            {
                Destroy(hotKey);
            }
        }

        private void CreateHotkey(Collider2D collision, Enemy enemy)
        {
            if (!_canCreateHotKeys)
                return;
            
            if (keyCodes.Count <= 0)
            {
                Debug.Log("Not enough hot keys in the key code list.");
                return;
            }

            if (_cloneAttackReleased)
                return;

            GameObject newHotKey =
                Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
            createdHotKeys.Add(newHotKey);

            KeyCode chosenKey = keyCodes[Random.Range(0, keyCodes.Count)];

            keyCodes.Remove(chosenKey);

            BlackHoleHotkeyController newHotkeyScript = newHotKey.GetComponent<BlackHoleHotkeyController>();

            newHotkeyScript.SetupHotKey(chosenKey, enemy.transform, this); //Attach each enemy to each hot key 
        }

        private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

        public void AddEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform);

        public bool ContainsEnemy(Transform enemyTransform) => targets.Contains(enemyTransform);
    }
}