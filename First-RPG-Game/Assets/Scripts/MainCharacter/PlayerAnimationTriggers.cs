using Audio;
using Enemies;
using Skills;
using Stats;
using UnityEngine;

namespace MainCharacter
{
    public class PlayerAnimationTriggers : MonoBehaviour
    {
        private Player Player => GetComponentInParent<Player>();

        private void AnimationTrigger()
        {
            Player.AnimationTrigger();
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.attackCheck.position, Player.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var enemy = hit.GetComponent<Enemy>();
                PlaySwordSound(enemy);

                if (enemy is not null)
                {
                    Player.Stats.DoDamage(enemy.GetComponent<EnemyStats>());
                    enemy.FX.CreateHitFx(enemy.transform, false);
                }
            }
        }

        private void StunAttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.attackCheck.position, Player.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var enemy = hit.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.FX.CreateHitFx(enemy.transform, false);
                    enemy.IsCanBeStunned(true);
                    enemy.GetComponent<CharacterStats>().TakeDamage(Player.Stats.damage.ModifiedValue, Color.gray);
                }
            }
        }

        private void ThrowSword()
        {
            SkillManager.Instance.Sword.CreateSword();
            SoundManager.PlaySFX("Attack", 4, true);
        }
        private void PlayCatchSwordSound()
        {
            SkillManager.Instance.Sword.CreateSword();
            SoundManager.PlaySFX("Hurt", null, true);
        }

        private void PlaySwordSound(Enemy enemy)
        {
            if (enemy is not null)
            {
                SoundManager.PlaySFX("Attack", 6, true);
            }
            else
            {

                SoundManager.PlaySFX("Attack", 0, true);
            }
        }

        private void PlayDashSound()
        {
            SoundManager.PlaySFX("Attack", 3, true);
        }

        public void PlayFootstepSound()
        {
            SoundManager.PlaySFX("Footstep", null, true);
        }
        public void PlayDeadSound()
        {
            SoundManager.PlaySFX("Dead", null, false);
        }
    }
}