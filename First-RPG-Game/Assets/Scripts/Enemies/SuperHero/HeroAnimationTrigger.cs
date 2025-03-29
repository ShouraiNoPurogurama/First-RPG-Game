using Enemies;
using MainCharacter;
using Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class HeroAnimationTrigger : MonoBehaviour
{
    private Enemy Enemy => GetComponentInParent<Enemy>();

    private void AnimationTrigger()
    {
        Enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Enemy.attackCheck.position, Enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            var player = hit.GetComponent<Player>();
            if (player)
            {
                Enemy.Stats.DoDamage(player.GetComponent<PlayerStats>());
            }
        }
    }

    private void OpenCounterWindow() => Enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => Enemy.CloseCounterAttackWindow();

    private void SpecialAttackTrigger()
    {
        Enemy.AnimationSpecialAttackTrigger();
    }

    private void SecondaryAttackTrigger()
    {
        Enemy.SecondaryAnimationSpecialAttackTrigger();
    }

    private void BusyMarker()
    {
        Enemy.BusyMarker();
    }
}
