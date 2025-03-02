using MainCharacter;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Buff")]
public class ItemData_Buff : ItemData
{
    [Header("Major stats")]
    public int hpPlus; // 
    public int speedPlus; //     


    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        playerStats.RecoverHP(hpPlus);
    }

    
}
