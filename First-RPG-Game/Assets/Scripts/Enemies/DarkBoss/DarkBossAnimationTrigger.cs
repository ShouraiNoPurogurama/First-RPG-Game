using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.DarkBoss
{
    public class DarkBossAnimationTrigger : MonoBehaviour
    {
        private DarkBoss DarkBoss => GetComponentInParent<DarkBoss>();
        private void AnimationTrigger()
        {
            DarkBoss.AnimationFinishTrigger();
        }

        private void TeleportTrigger()
        {
            DarkBoss.FindPosition();
        }
        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(DarkBoss.attackCheck.position, DarkBoss.attackCheckRadius);
            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    DarkBoss.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }
        private void OpenCounterWindow() => DarkBoss.OpenCounterAttackWindow();
        private void CloseCounterWindow() => DarkBoss.CloseCounterAttackWindow();

    }
}
