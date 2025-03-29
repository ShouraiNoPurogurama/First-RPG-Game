using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.Giant
{
    public class GiantAnimationTrigger : MonoBehaviour
    {
        private Giant Giant => GetComponentInParent<Giant>();
        private void AnimationTrigger()
        {
            Giant.AnimationFinishTrigger();
        }
        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Giant.attackCheck.position, Giant.attackCheckRadius);
            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    Giant.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }
        private void OpenCounterWindow() => Giant.OpenCounterAttackWindow();
        private void CloseCounterWindow() => Giant.CloseCounterAttackWindow();
    }
}

