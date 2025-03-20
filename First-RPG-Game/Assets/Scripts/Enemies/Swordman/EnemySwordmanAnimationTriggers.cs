using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.Swordman
{
    public class EnemySwordmanAnimationTriggers : MonoBehaviour
    {
        private Enemy Enemy => GetComponentInParent<Enemy>();
        private void AnimationTrigger()
        {
            Enemy.AnimationFinishTrigger();
        }
    }
 


}
