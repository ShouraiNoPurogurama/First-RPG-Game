using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class Stat
    {
        [SerializeField] private int baseValue;

        /// <summary>
        /// Collection of additional modifications (from equipments,...) that adjust the base stat value
        /// </summary>
        [SerializeField] public List<int> modifiers;

        public int ModifiedValue
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
        public int GetValue()
        {
            int finalValue = baseValue;

            foreach (int modifier in modifiers)
            {
                finalValue += modifier;
            }

            return finalValue;
        }
    }
}