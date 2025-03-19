using UnityEngine;

namespace Inventory_and_Item.ItemEffect
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
    public class ItemEffect : ScriptableObject
    {
        public virtual void ExecuteEffect(Transform enemyPosition)
        {
            Debug.Log("Effected");
        }
    }
}