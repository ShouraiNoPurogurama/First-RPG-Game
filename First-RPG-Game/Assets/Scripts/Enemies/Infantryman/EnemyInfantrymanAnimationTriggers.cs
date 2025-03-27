using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.Infantryman
{
    public class EnemyInfantrymanAnimationTriggers : MonoBehaviour
    {
        private Enemy Enemy => GetComponentInParent<Enemy>();
        private void AnimationTrigger()
        {
            Enemy.AnimationFinishTrigger();
        }
    }
 


}
