using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeekerAnimationTrigger : MonoBehaviour
    {
        private Enemy Enemy => GetComponentInParent<Enemy>();

        private void AnimationTrigger()
        {
            Enemy.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Enemy.attackCheck.position, Enemy.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    Enemy.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }

        private void OpenCounterWindow() => Enemy.OpenCounterAttackWindow();
        private void CloseCounterWindow() => Enemy.CloseCounterAttackWindow();

        private void SpecialAttackTrigger()
        {
            Enemy.AnimationSpecialAttackTrigger();
        }

        private void SecondaryAttackTrigger()
        {
            Enemy.SecondaryAnimationSpecialAttackTrigger();
        }

        private void BusyMarker()
        {
            Enemy.BusyMarker();
        }
    }
}
