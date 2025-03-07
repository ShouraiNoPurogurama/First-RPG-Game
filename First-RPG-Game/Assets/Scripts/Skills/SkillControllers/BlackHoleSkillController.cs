using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Enemies;
using MainCharacter;
using UnityEngine;
using UnityEngine.UIElements;
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
        private float _blackHoleTimer;
        private float _pullSpeed = 3f;
        private bool _playerCanDisappear = true;

        private List<Enemy> _enemiesInBlackHole = new();

        [Header("Clone attack info")]
        private int _attackAmountPerTrigger = 4;

        private int _totalTriggerableAmount = 3;
        private int _totalTriggerableAmountLeft;
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

        private Dictionary<KeyCode, Transform> _hotkeyToEnemy = new();

        private Dictionary<KeyCode, BlackHoleHotkeyController> _newHotkeyScripts = new();

        public bool PlayerCanExitState { get; private set; }

        public void SetupBlackHole(float maxSize, float growSpeed, float shrinkSpeed, int attackAmount, float cloneAttackCooldown,
            float blackHoleDuration, int triggerAttackMount)
        {
            _maxSize = maxSize;
            _growSpeed = growSpeed;
            _shrinkSpeed = shrinkSpeed;
            _attackAmountPerTrigger = attackAmount;
            _cloneAttackCooldown = cloneAttackCooldown;
            _blackHoleTimer = blackHoleDuration;
            _totalTriggerableAmount = triggerAttackMount;

            _virtualCamera = GameObject.Find("Virtual Camera");
            _cinemachineVirtualCamera = _virtualCamera.GetComponent<CinemachineVirtualCamera>();
            _originalCameraTarget = _cinemachineVirtualCamera.m_Follow;
            _totalTriggerableAmountLeft = _totalTriggerableAmount;

            if (SkillManager.Instance.Clone.crystalInsteadOfClone)
            {
                _playerCanDisappear = false;
            }
        }

        private void Update()
        {
            UpdateTimers();

            if (_blackHoleTimer <= 0)
            {
                FinishBlackHoleAbility();
            }

            HandleBlackHoleScaling();
            HandleEnemyPulling();
            HandleAttackLogic();

            if (Input.GetKeyDown(KeyCode.R))
            {
                FinishBlackHoleAbility();
            }
        }

        private void UpdateTimers()
        {
            _cloneAttackTimer -= Time.deltaTime;
            _blackHoleTimer -= Time.deltaTime;
        }

        private void HandleBlackHoleScaling()
        {
            if (_canGrow && !_canShrink)
            {
                GrowBlackHole();
            }
            if (_canShrink)
            {
                ShrinkBlackHole();
            }
        }

        private void HandleEnemyPulling()
        {
            PullEnemiesToCenter();
            PullHotkeysTogether();
        }

        private void HandleAttackLogic()
        {
            if (transform.localScale.x >= _maxSize * 0.98f && _hotkeyToEnemy.Count == 1 &&
                _totalTriggerableAmountLeft == _totalTriggerableAmount)
            {
                TriggerFullBlackHoleAttack();
            }
            else
            {
                HandleManualTriggerAttacks();
            }
        }

        private void GrowBlackHole()
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.localScale.x - _maxSize) < 0.01f)
            {
                transform.localScale = new Vector2(_maxSize, _maxSize);
                _canGrow = false;
            }
        }

        private void ShrinkBlackHole()
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void TriggerFullBlackHoleAttack()
        {
            KeyCode key = _hotkeyToEnemy.Keys.First();

            StartCoroutine(WaitForColorChange(key, true));

            _attackAmountPerTrigger *= _totalTriggerableAmount;
            _cloneAttackCooldown *= 0.8f;

            TriggerCloneAttack(key);

            _blackHoleTimer = _attackAmountPerTrigger * _cloneAttackCooldown + 0.4f;

            _totalTriggerableAmountLeft = 0;
            _canCreateHotKeys = false;
        }

        private void HandleManualTriggerAttacks()
        {
            foreach (var key in _hotkeyToEnemy.Keys)
            {
                if (Input.GetKeyDown(key) && _totalTriggerableAmountLeft > 0)
                {
                    bool destroyHotKey = _totalTriggerableAmountLeft == 1; // If last attack, destroy the hotkey

                    StartCoroutine(WaitForColorChange(key, destroyHotKey));
                    TriggerCloneAttack(key);

                    _totalTriggerableAmountLeft--;

                    if (_totalTriggerableAmountLeft <= 0)
                    {
                        _blackHoleTimer = _attackAmountPerTrigger * _cloneAttackCooldown + 0.8f;
                    }

                    break;
                }
            }
        }


        private IEnumerator WaitForColorChange(KeyCode key, bool destroy)
        {
            yield return StartCoroutine(_newHotkeyScripts[key].ChangeColorTemporarily(0.25f));

            //After color change finished, destroy hot keys
            if (destroy)
            {
                DestroyHotKeys();
            }
        }

        private void ExtendBlackHoleDuration(float seconds)
        {
            _blackHoleTimer += seconds;
        }

        private void TriggerCloneAttack(KeyCode key)
        {
            if (!_hotkeyToEnemy.ContainsKey(key) || _totalTriggerableAmountLeft <= 0)
                return;

            Transform enemyTarget = _hotkeyToEnemy[key];

            if (_playerCanDisappear)
            {
                _playerCanDisappear = false;
                PlayerManager.Instance.player.SetTransparent(true);
            }
            
            //Start attack sequence
            StartCoroutine(DelayedCloneAttack(enemyTarget));

            ExtendBlackHoleDuration(_attackAmountPerTrigger * _cloneAttackCooldown + 0.4f);
        }


        private IEnumerator DelayedCloneAttack(Transform enemyTarget)
        {
            for (int i = 0; i < _attackAmountPerTrigger; i++)
            {
                if (_cloneAttackTimer <= 0)
                {
                    _cloneAttackTimer = _cloneAttackCooldown;

                    if (SkillManager.Instance.Clone.crystalInsteadOfClone)
                    {
                        SkillManager.Instance.Crystal.CreateCrystal();
                        SkillManager.Instance.Crystal.CurrentCrystalChooseRandomTarget();
                    }
                    else
                    {
                        float xOffset = Random.Range(0, 100) > 50 ? 1.7f : -1.7f;
                        SkillManager.Instance.Clone.CreateClone(enemyTarget, new Vector3(xOffset, 0));
                    }
                    _cinemachineVirtualCamera.Follow = enemyTarget;

                    if (Mathf.Approximately(enemyTarget.transform.position.z, 10))
                    {
                        _cinemachineVirtualCamera.Follow = _originalCameraTarget;
                    }
                }

                yield return new WaitForSeconds(_cloneAttackCooldown);
            }
        }


        // private void ReleaseCloneAttack()
        // {
        //     if (targets.Count == 0) 
        //         return;
        //
        //     if (_totalTriggerableAmountLeft <= 0)
        //     {
        //         DestroyHotKeys();
        //     }
        //     
        //     _cloneAttackReleased = true;
        //     _canCreateHotKeys = false;
        //     
        //     PlayerManager.Instance.player.SetTransparent(true);
        // }

        // private void CloneAttackLogic()
        // {
        //     if (_cloneAttackTimer <= 0 && _cloneAttackReleased)
        //     {
        //         _cloneAttackTimer = _cloneAttackCooldown;
        //         
        //         int randomIndex = Random.Range(0, targets.Count - 1);
        //
        //         if (randomIndex < 0) randomIndex = 0;
        //
        //         float xOffset = Random.Range(0, 100) > 50 ? 1.5f : -1.5f;
        //
        //         SkillManager.Instance.Clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
        //
        //         _cinemachineVirtualCamera.Follow = targets[randomIndex].transform;
        //             
        //         _attackAmountLeft--;
        //         
        //         //internally floor the result
        //         var newTotalTriggerableAmountLeft = _attackAmountLeft / _attackAmountPerTrigger;
        //         
        //         if (newTotalTriggerableAmountLeft < _totalTriggerableAmountLeft)
        //         {
        //             _totalTriggerableAmountLeft = newTotalTriggerableAmountLeft;
        //             PlayerManager.Instance.player.SetTransparent(false);
        //         }
        //         
        //         if (_totalTriggerableAmountLeft <= 0)
        //         {
        //             Invoke("FinishBlackHoleAbility", 1f);
        //         }
        //     }
        // }

        private void FinishBlackHoleAbility()
        {
            DestroyHotKeys();
            _canShrink = true;
            _cloneAttackReleased = false;
            PlayerCanExitState = true;
            _cinemachineVirtualCamera.Follow = _originalCameraTarget;

            PlayerManager.Instance.player.SetTransparent(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy is not null)
            {
                enemy.FreezeTime(true);

                CreateHotkey(collision, enemy);

                _enemiesInBlackHole.Add(enemy);
            }
        }

        private void PullEnemiesToCenter()
        {
            foreach (var enemy in _enemiesInBlackHole)
            {
                if (!enemy || Mathf.Approximately(enemy.transform.position.z, 10)) continue;

                Vector3 direction = (transform.position - enemy.transform.position).normalized;

                if (enemy.transform.position.y < transform.position.y - 1)
                {
                    direction.y += .3f;
                }

                enemy.transform.position += direction * (_pullSpeed * Time.deltaTime);
                
                if (Mathf.Approximately(enemy.transform.position.z, 10))
                {
                    _cinemachineVirtualCamera.Follow = _originalCameraTarget;
                }
            }
        }

        private void PullHotkeysTogether()
        {
            foreach (var hotkey in _newHotkeyScripts.Values)
            {
                if (!hotkey) continue;

                // Vector3 direction = (transform.position - hotkey.transform.position).normalized;
                hotkey.transform.position = hotkey.Enemy.transform.position + new Vector3(0, 2);
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
            if (!_canCreateHotKeys || keyCodes.Count <= 0 || _cloneAttackReleased)
                return;

            GameObject newHotKey =
                Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
            createdHotKeys.Add(newHotKey);

            KeyCode chosenKey = keyCodes[Random.Range(0, keyCodes.Count)];
            keyCodes.Remove(chosenKey);

            BlackHoleHotkeyController newHotkeyScript = newHotKey.GetComponent<BlackHoleHotkeyController>();
            newHotkeyScript.SetupHotKey(chosenKey, enemy.transform, this);
            _newHotkeyScripts.Add(chosenKey, newHotkeyScript);

            //Store hotkey and enemy target
            _hotkeyToEnemy[chosenKey] = enemy.transform;
        }


        private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

        public void AddEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform);

        public bool ContainsEnemy(Transform enemyTransform) => targets.Contains(enemyTransform);
    }
}