using System.Text;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
    Buff,
    Gold
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [Range(0, 100)]
    public float dropChance;
    protected StringBuilder sb = new StringBuilder();
    public virtual string GetDescription()
    {
        return "";
    }
}