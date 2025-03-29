using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.Boss
{
    public class EarthBossAnimationTriggers : MonoBehaviour
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
                    //Vector2 knockBackValue = boss.knockBackPlayer;
                    //player.BossAttackPlayerKnock(knockBackValue);
                    boss.Stats.DoDamageDontKnock(player.GetComponent<PlayerStats>());
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
                    //Debug.Log("anim thuc te thuc thi:   " + boss.knockBackPlayer);
                    Vector2 knockBackValue = boss.knockBackPlayer;
                    player.BossAttackPlayerKnock(knockBackValue);
                    boss.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
            boss.knockBackPlayer = new Vector2(1, 5);
        }

        private void OpenCounterWindow() => boss.OpenCounterAttackWindow();
        private void CloseCounterWindow() => boss.CloseCounterAttackWindow();
    }
}
