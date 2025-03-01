using Enemies;
using MainCharacter;
using UnityEngine;

public class Strike_Controller : MonoBehaviour
{
    private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            _playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
