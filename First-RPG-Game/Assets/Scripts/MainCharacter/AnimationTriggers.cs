using Enemies;
using Skills;
using UnityEngine;

namespace MainCharacter
{
    public class AnimationTriggers : MonoBehaviour
    {
        private Player Player => GetComponentInParent<Player>();
    
        private void AnimationTrigger()
        {
            Player.AnimationTrigger();
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.attackCheck.position, Player.attackCheckRadius);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() is not null)
                {
                    hit.GetComponent<Enemy>().Damage();
                }
            }
        }

        private void ThrowSword()
        {
            SkillManager.Instance.Sword.CreateSword();
        }
    }
}
