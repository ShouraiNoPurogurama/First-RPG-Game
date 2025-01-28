using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    private Player Player => GetComponentInParent<Player>();
    
    private void AnimationTrigger()
    {
        Player.AnimationTrigger();
    }
}
