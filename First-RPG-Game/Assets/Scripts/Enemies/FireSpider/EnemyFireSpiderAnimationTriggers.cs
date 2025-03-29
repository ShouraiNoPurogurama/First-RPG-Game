using Audio;
using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.FireSpider
{
    public class EnemyFireSpiderAnimationTriggers : MonoBehaviour
    {
        private EnemyFireSpider fireSpider => GetComponentInParent<EnemyFireSpider>();

        private void AnimationTrigger()
        {
            //Debug.Log("AnimationTrigger() called!");
            fireSpider.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            SoundManager.PlaySFX("FireSpider", 1, true);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(fireSpider.attackCheck.position, fireSpider.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    fireSpider.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }
        private void playStep()
        {
            SoundManager.PlaySFX("FireSpider", 0, true);

        }
        private void OpenCounterWindow() => fireSpider.OpenCounterAttackWindow();
        private void CloseCounterWindow() => fireSpider.CloseCounterAttackWindow();
    }
}
