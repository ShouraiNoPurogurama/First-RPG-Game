using Stats;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterStats>() != null)
        {
            Debug.Log("DeadZone: " + collision.name);
            collision.GetComponent<CharacterStats>().KillEntity();
            collision.GetComponent<CharacterStats>().currentHp = 0;
        }
        else
            Destroy(collision.gameObject);
    }
}
