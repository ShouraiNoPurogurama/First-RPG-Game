using Enemies.FireMage;
using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.FireMage
{
    public class EnemyFireMageAnimationTriggers : MonoBehaviour
    {
        private EnemyFireMage fireMage => GetComponentInParent<EnemyFireMage>();

        private void AnimationTrigger()
        {
            //Debug.Log("AnimationTrigger() called!");
            fireMage.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {/*
            Collider2D[] colliders = Physics2D.OverlapCircleAll(fireMage.attackCheck.position, fireMage.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    fireMage.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }*/
        }

        private void OpenCounterWindow() => fireMage.OpenCounterAttackWindow();
        private void CloseCounterWindow() => fireMage.CloseCounterAttackWindow();
    }
}
