using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.FireWorm
{
    public class EnemyFireWormAnimationTriggers : MonoBehaviour
    {
        private EnemyFireWorm fireWorm => GetComponentInParent<EnemyFireWorm>();

        private void AnimationTrigger()
        {
            //Debug.Log("AnimationTrigger() called!");
            fireWorm.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(fireWorm.attackCheck.position, fireWorm.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    fireWorm.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }

        private void OpenCounterWindow() => fireWorm.OpenCounterAttackWindow();
        private void CloseCounterWindow() => fireWorm.CloseCounterAttackWindow();
    }
}
