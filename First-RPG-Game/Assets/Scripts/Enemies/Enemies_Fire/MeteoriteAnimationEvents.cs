using Enemies.FireMage;
using UnityEngine;

namespace Enemies.Enemies_Fire
{
    public class MeteoriteAnimationEvents : MonoBehaviour
    {
        //private MeteoriteSkillFall meteoriteSkillFall => GetComponentInParent<MeteoriteSkillFall>();
        public void DestroyAfterAnimation()
        {
            Destroy(gameObject);
        }
    }
}
