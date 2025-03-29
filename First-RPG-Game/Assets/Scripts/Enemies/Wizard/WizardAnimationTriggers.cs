// WizardAnimationTriggers.cs
using UnityEngine;

namespace Enemies.Wizard
{
    public class WizardAnimationTriggers : MonoBehaviour
    {
        private EnemyWizard Wizard => GetComponentInParent<EnemyWizard>();

        private void AnimationTrigger()
        {
            Wizard.AnimationFinishTrigger();
        }


    }
}