using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class EnemyMagicSkeletonAnimationTrigger : MonoBehaviour
    {
        private Enemy_Magic_Skeleton Skeleton => GetComponentInParent<Enemy_Magic_Skeleton>();

        private void AnimationTrigger()
        {
            Skeleton.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Skeleton.attackCheck.position, Skeleton.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    Skeleton.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }
        private void SpecialAttackTrigger()
        {
            Skeleton.AnimationSpecialAttackTrigger();
        }
        private void OpenCounterWindow() => Skeleton.OpenCounterAttackWindow();
        private void CloseCounterWindow() => Skeleton.CloseCounterAttackWindow();
    }
}
