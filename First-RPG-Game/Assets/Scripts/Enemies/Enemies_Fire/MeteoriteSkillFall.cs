using MainCharacter;
using UnityEngine;

namespace Enemies.Enemies_Fire
{
    public class MeteoriteSkillFall : MonoBehaviour
    {
        public float fallSpeed = 2f;
        public Transform groundCheck;
        public Transform wallCheck;  
        public LayerMask whatIsGround; 
        public float checkDistance = 0.2f;
        private Animator animator;
        private bool isExploding = false;
        //private MeteoriteAnimationEvents meteoriteEvents;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //Animator animator = meteorite.GetComponent<Animator>();
            animator = GetComponent<Animator>();
            //meteoriteEvents = GetComponent<MeteoriteAnimationEvents>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isExploding) return;

            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            Debug.Log(IsGroundDetected() + "............" + IsWallDetected());
            if (IsGroundDetected() || IsWallDetected())
            {
                TriggerAnimation("Explode");
                //Destroy(gameObject);
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
                    TriggerAnimation("Explode");
                    //Destroy(gameObject);
                    return;
                }
                //Debug.Log("Cham roi ne");
                player.Stats.TakeDamage(30, Color.yellow);
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

        private bool IsWallDetected()
        {
            return Physics2D.Raycast(wallCheck.position, Vector2.right, checkDistance, whatIsGround);
        }
        private void TriggerAnimation(string animName)
        {
            isExploding = true;
            animator.SetTrigger(animName);
        }
    }
}
