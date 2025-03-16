using UnityEngine;

namespace Skills.SkillControllers.Water_Map
{
    public class WhirlpoolSkill : MonoBehaviour
    {
        [SerializeField] private GameObject whirlpoolPrefab;
        public static WhirlpoolSkill Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }
        public void SpawnWhirlpool(Transform playerTransform, float offsetY)
        {
            Vector2 spawnPosition = new Vector2(
                playerTransform.position.x,
                playerTransform.position.y + offsetY
            );

            GameObject whirlpool = Instantiate(whirlpoolPrefab, spawnPosition, Quaternion.identity);
            whirlpool.transform.SetParent(playerTransform);
            Rigidbody2D whirlpoolRb = whirlpool.GetComponent<Rigidbody2D>();
            if (whirlpoolRb != null)
            {
                whirlpoolRb.bodyType = RigidbodyType2D.Kinematic;
                whirlpoolRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            Collider2D whirlpoolCollider = whirlpool.GetComponent<Collider2D>();
            whirlpoolCollider.enabled = false;
            whirlpool.SetActive(true);
        }
    }
}
