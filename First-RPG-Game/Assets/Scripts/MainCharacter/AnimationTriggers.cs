using Enemies;
using Skills;
using UnityEngine;

namespace MainCharacter
{
    public class AnimationTriggers : MonoBehaviour
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
                    enemy.FX.CreateHitFx(enemy.transform, false);
                    enemy.Damage();
                }
            }
        }
        
        private void StunAttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.attackCheck.position, Player.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var enemy = hit.GetComponent<Enemy>();
                enemy?.IsCanBeStunned(true);
            }
        }

        private void ThrowSword()
        {
            SkillManager.Instance.Sword.CreateSword();
        }
    }
}
