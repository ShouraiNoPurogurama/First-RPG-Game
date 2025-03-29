using Stats;
using UnityEngine;

namespace Skills.SkillControllers.Water_Map
{
    public class WaterBall_Controller : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private string targetLayerName = "Player";

        [SerializeField] private float xVelocity;
        [SerializeField] private Rigidbody2D rb;

        [SerializeField] private bool canMove;
        [SerializeField] private bool flipped;

        [SerializeField] private float whirlpoolOffsetY;

        public CharacterStats myStats;

        private void Update()
        {
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        }

        public void SetupWaterBall(float _speed, CharacterStats _myStats)
        {
            xVelocity = _speed;
            myStats = _myStats;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
            {
                var collisionStats = collision.GetComponent<CharacterStats>();

                if (myStats == null)
                {
                    Debug.LogError($"{name}: myStats is NULL. Did you forget to call SetupWaterBall()?");
                }

                if (collisionStats == null)
                {
                    Debug.LogError($"{name}: The object {collision.gameObject.name} does NOT have CharacterStats, but is on the target layer '{targetLayerName}'.");
                }

                if (myStats != null && collisionStats != null)
                {
                    myStats.DoDamage(collisionStats);

                    if (WhirlpoolSkill.Instance != null)
                    {
                        WhirlpoolSkill.Instance.SpawnWhirlpool(collision.transform, whirlpoolOffsetY);
                    }
                    else
                    {
                        Debug.LogError($"{name}: WhirlpoolSkill.Instance is NULL. Make sure WhirlpoolSkill is in the scene and set up properly.");
                    }
                }

                Destroy(gameObject);
            }
        }

        public void FlipWaterBall()
        {
            if (flipped)
            {
                return;
            }
            xVelocity *= -1;
            flipped = true;
            transform.Rotate(0, 180, 0);
            targetLayerName = "Enemy";
        }
    }
}
