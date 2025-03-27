using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.FireQueen
{
    public class EnemyFireQueenAnimationTriggers : MonoBehaviour
    {
        private EnemyFireQueen fireQueen => GetComponentInParent<EnemyFireQueen>();

        private void AnimationTrigger()
        {
            //Debug.Log("AnimationTrigger() called!");
            fireQueen.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(fireQueen.attackCheck.position, fireQueen.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    fireQueen.Stats.DoDamage(player.GetComponent<PlayerStats>());
                }
            }
        }

        private void OpenCounterWindow() => fireQueen.OpenCounterAttackWindow();
        private void CloseCounterWindow() => fireQueen.CloseCounterAttackWindow();
    }
}
