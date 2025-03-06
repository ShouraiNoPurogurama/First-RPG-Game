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
                    enemy.GetComponent<CharacterStats>().TakeDamage(Player.Stats.damage.ModifiedValue);
                }
            }
        }

        private void ThrowSword()
        {
            SkillManager.Instance.Sword.CreateSword();
        }
    }
}