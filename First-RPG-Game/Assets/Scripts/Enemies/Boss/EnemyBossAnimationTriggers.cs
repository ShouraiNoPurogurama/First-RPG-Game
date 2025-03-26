using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.Boss
{
    public class EnemyBossAnimationTriggers : MonoBehaviour
    {
        private EnemyBoss boss => GetComponentInParent<EnemyBoss>();

        private void AnimationTrigger()
        {
            //Debug.Log("AnimationTrigger() called!");
            boss.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(boss.attackCheck.position, boss.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    boss.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }
        private void PlayerKnock()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(boss.attackCheck.position, boss.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    player.BossAttackPlayerKnock(boss.knockBackPlayer);
                    //boss.knockBackPlayer = new Vector2(5, 5);
                }
            }
        }

        private void OpenCounterWindow() => boss.OpenCounterAttackWindow();
        private void CloseCounterWindow() => boss.CloseCounterAttackWindow();
    }
}
