using Audio;
using MainCharacter;
using UnityEngine;

namespace Enemies.Enemies_Fire
{
    public class MeteoriteSkillFall : MonoBehaviour
    {
        public float fallSpeed = 10f;
        public Transform groundCheck;
        public Transform wallCheck;  
        public LayerMask whatIsGround; 
        public float checkDistance = 0.2f;
        private Animator animator;
        private bool isExploding = false;
        private Rigidbody2D rb;
        //private MeteoriteAnimationEvents meteoriteEvents;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            //Animator animator = meteorite.GetComponent<Animator>();
            animator = GetComponent<Animator>();
            rb.linearVelocity = Vector2.down * fallSpeed;
            //meteoriteEvents = GetComponent<MeteoriteAnimationEvents>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isExploding) return;
           
            rb.linearVelocity = Vector2.down * fallSpeed;
            
            if (IsGroundDetected())
            {
                rb.linearVelocity = Vector2.zero;
                SoundManager.PlaySFX("FireMage", 2, true);
                TriggerAnimation("Explode");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isExploding) return;
            //Debug.Log("Cham roi");
            Player player = collision.GetComponent<Player>();
            //Debug.Log(player);
            if (player != null)
            {
                //Debug.Log(player.StateMachine.CurrentState);
                if (player.StateMachine.CurrentState is PlayerCounterAttackState)
                {
                    rb.linearVelocity = Vector2.zero;
                    SoundManager.PlaySFX("FireMage", 2, true);
                    TriggerAnimation("Explode");
                    //Destroy(gameObject);  
                    return;
                }
                //Debug.Log("Cham roi ne");
                player.Stats.TakeDamage(30, Color.yellow);
                rb.linearVelocity = Vector2.zero;
                SoundManager.PlaySFX("FireMage", 2, true);
                TriggerAnimation("Explode");
                //Destroy(gameObject);
            }
        

        }
        void OnBecameInvisible() 
        {
            Destroy(gameObject);
        }
       
        private bool IsGroundDetected()
        {
            return Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, whatIsGround);
        }
        private void TriggerAnimation(string animName)
        {
            isExploding = true;
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger(animName);
            //StartCoroutine(DestroyAfterAnimation());
        }
    }
}
