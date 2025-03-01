using UnityEngine;
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect/thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, enemyPosition.position, Quaternion.identity);
        Destroy(newThunderStrike, .5f);
    }
}
