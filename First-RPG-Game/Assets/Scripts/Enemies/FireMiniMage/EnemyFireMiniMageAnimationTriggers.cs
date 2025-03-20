using Enemies.FireMiniMage;
using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class EnemyFireMiniMageAnimationTriggers : MonoBehaviour
    {
        private EnemyFireMiniMage FireMiniMage => GetComponentInParent<EnemyFireMiniMage>();

        private void AnimationTrigger()
        {
            //Debug.Log("AnimationTrigger() called!");
            FireMiniMage.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(FireMiniMage.attackCheck.position, FireMiniMage.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    FireMiniMage.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }

        private void OpenCounterWindow() => FireMiniMage.OpenCounterAttackWindow();
        private void CloseCounterWindow() => FireMiniMage.CloseCounterAttackWindow();
    }
}
