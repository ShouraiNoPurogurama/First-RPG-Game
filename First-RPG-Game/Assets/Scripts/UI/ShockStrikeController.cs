using Stats;
using UnityEngine;

namespace UI
{
    public class ShockStrikeController : MonoBehaviour
    {
        [SerializeField] private CharacterStats targetStats;
        [SerializeField] private float speed;
        private int _damage = 5;

        private Animator _animator;
        private bool _triggered;

        public void SetUp(int damage, CharacterStats targetStats)
        {
            _damage = damage;
            this.targetStats = targetStats;
        }
    
        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            if (!targetStats || _triggered)
            {
                return;
            }
        
            transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
            transform.right = transform.position - targetStats.transform.position;
        
            if (Vector2.Distance(transform.position, targetStats.transform.position) <= 0.1f)
            {
                _animator.transform.localRotation = Quaternion.identity;
                transform.localRotation = Quaternion.identity;
                transform.localScale = new Vector3(3,3);
                _animator.transform.localPosition = new Vector3(0, .2f);
            
                Invoke("DamageAndSelfDestroy", .2f);
                _triggered = true;
            
                Destroy(gameObject, .4f);
            }
        }

        private void DamageAndSelfDestroy()
        {
            targetStats.ApplyShock(true);
            targetStats.TakeDamage(_damage, Color.yellow);
            _animator.SetTrigger("Hit");
        }
    }
}
