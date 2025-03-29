using Audio;
using Enemies.FireMiniMage;
using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class EnemyFireMiniMageAnimationTriggers : MonoBehaviour
    {
        private EnemyFireMiniMage FireMiniMage => GetComponentInParent<EnemyFireMiniMage>();

        private void AnimationTrigger()
        {
            //Debug.Log("AnimationTrigger() called!");
            FireMiniMage.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            SoundManager.PlaySFX("FireMiniMage", 0, true);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(FireMiniMage.attackCheck.position, FireMiniMage.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    FireMiniMage.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }
        private void AnimationFireballAttackTrigger()
        {
            //base.AnimationFireballAttackTrigger();

            Vector3 spawnPosition = FireMiniMage.attackCheck.position - new Vector3(1f * FireMiniMage.FacingDir, 0, 0);
            SoundManager.PlaySFX("FireMiniMage", 1, true);
            GameObject newfireball = Instantiate(FireMiniMage.fireballPrefab, spawnPosition, Quaternion.identity);

            // Thiết lập hướng bay cho fireball
            newfireball.GetComponent<FireballController>().SetupFireball(FireMiniMage.fireballSpeed * FireMiniMage.FacingDir, FireMiniMage.Stats);
        }

        private void OpenCounterWindow() => FireMiniMage.OpenCounterAttackWindow();
        private void CloseCounterWindow() => FireMiniMage.CloseCounterAttackWindow();
    }
}
