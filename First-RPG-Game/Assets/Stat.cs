using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    [SerializeField] public List<int> modifiers;

    public int FinalValue
    {
        get
        {
            int finalValue = baseValue;

            foreach (int modifier in modifiers)
            {
                finalValue += modifier;
            }

            return finalValue;
        }
    }

    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
    }

    public void SetDefaultValue(int value)
    {
        baseValue = value;
    }
}