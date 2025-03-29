using Audio;
using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.FireSnake
{
    public class EnemyFireSnakeAnimationTriggers : MonoBehaviour
    {
        private EnemyFireSnake fireSnake => GetComponentInParent<EnemyFireSnake>();

        private void AnimationTrigger()
        {
            //Debug.Log("AnimationTrigger() called!");
            fireSnake.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            SoundManager.PlaySFX("FireSnake", 1, true);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(fireSnake.attackCheck.position, fireSnake.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    fireSnake.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }
        private void playStep()
        {
            SoundManager.PlaySFX("FireSnake", 0, true);
        }
        private void OpenCounterWindow() => fireSnake.OpenCounterAttackWindow();
        private void CloseCounterWindow() => fireSnake.CloseCounterAttackWindow();
    }
}
