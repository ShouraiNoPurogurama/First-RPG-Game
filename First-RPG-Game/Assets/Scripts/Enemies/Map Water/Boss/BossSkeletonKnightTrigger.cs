using MainCharacter;
using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Map_Water.Boss
{
    public class BossSkeletonKnightTrigger : MonoBehaviour
    {
        private BossSkeletonKnight BossKnight => GetComponentInParent<BossSkeletonKnight>();

        private Animator anim;

        [Header("Thông số cho kiểu spawn ngẫu nhiên quanh Player")]
        [SerializeField] private GameObject watterAttackFollowing;
        [SerializeField] private float randomRange = 1f;
        [SerializeField] private int numberOfExplosions = 3;
        [SerializeField] private float minDistanceBetweenExplosions = 1f;
        [SerializeField] private float explosionDelay = 0.5f;

        [Header("Turn on/Turn off Spawn Water Explosion")]
        [SerializeField] private bool isWaterExplosion = false;

        [Header("Setup SpawnFixedExplosions")]
        [SerializeField] private GameObject waterExplosionPrefab;
        [SerializeField] private int fixedExplosionCount = 5;
        [SerializeField] private float distanceBetweenFixedExplosions = 1f;

        private void AnimationTrigger()
        {
            BossKnight.AnimationFinishTrigger();
        }

        private void Ontransparent()
        {
            BossKnight.Stats.MakeInvincible(true);
            BossKnight.SetTransparent(true);
        }

        private void OnAppear()
        {
            BossKnight.Stats.MakeInvincible(false);
            BossKnight.SetTransparent(false);
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(BossKnight.attackCheck.position, BossKnight.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    BossKnight.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }

        private void SpecialAttackTrigger()
        {
            Debug.Log("Special Attack Trigger");
            Player player = FindObjectOfType<Player>();
            if (player)
            {
                Debug.Log("Player found");

                float healthPercent = (float)BossKnight.Stats.currentHp / BossKnight.Stats.GetMaxHealthValue();

                if (healthPercent <= 0.5f)
                {
                    isWaterExplosion = true;
                }

                if (isWaterExplosion)
                {
                    StartCoroutine(SpawnFixedExplosions());
                    StartCoroutine(SpawnExplosionsCoroutine(player));
                }
                else
                {
                    StartCoroutine(SpawnExplosionsCoroutine(player));
                }
            }
        }
        private IEnumerator SpawnFixedExplosions()
        {
            Vector3 groundPos = BossKnight.groundCheck.position;

            for (int i = 0; i < fixedExplosionCount; i++)
            {
                float offsetX = distanceBetweenFixedExplosions * i;
                Vector3 spawnPos = new Vector3(
                    groundPos.x + offsetX,
                    groundPos.y + .5f,
                    groundPos.z
                );

                Instantiate(waterExplosionPrefab, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
        }
        private IEnumerator SpawnExplosionsCoroutine(Player player)
        {
            List<Vector3> usedPositions = new List<Vector3>();

            for (int i = 0; i < numberOfExplosions; i++)
            {
                bool foundValidPos = false;
                int maxAttempts = 50;
                Vector3 spawnPos = Vector3.zero;

                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    float randomX = Random.Range(-randomRange, randomRange);
                    float randomY = Random.Range(-randomRange, 0);
                    spawnPos = player.transform.position + new Vector3(randomX, randomY, 0);

                    bool tooClose = false;
                    foreach (var pos in usedPositions)
                    {
                        if (Vector3.Distance(spawnPos, pos) < minDistanceBetweenExplosions)
                        {
                            tooClose = true;
                            break;
                        }
                    }
                    if (!tooClose)
                    {
                        foundValidPos = true;
                        break;
                    }
                }

                if (foundValidPos)
                {
                    Instantiate(watterAttackFollowing, spawnPos, Quaternion.identity);
                    usedPositions.Add(spawnPos);
                }
                yield return new WaitForSeconds(explosionDelay);
            }
        }

        private void OpenCounterWindow() => BossKnight.OpenCounterAttackWindow();
        private void CloseCounterWindow() => BossKnight.CloseCounterAttackWindow();
    }
}
