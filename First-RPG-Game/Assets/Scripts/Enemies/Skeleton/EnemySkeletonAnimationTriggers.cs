using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.Skeleton
{
    public class EnemySkeletonAnimationTriggers : MonoBehaviour
    {
        private EnemySkeleton Skeleton => GetComponentInParent<EnemySkeleton>();

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

        private void OpenCounterWindow() => Skeleton.OpenCounterAttackWindow();
        private void CloseCounterWindow() => Skeleton.CloseCounterAttackWindow();
    }
}
