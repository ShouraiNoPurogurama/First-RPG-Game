using MainCharacter;
using UnityEngine;

public class MeteoriteSkillFall : MonoBehaviour
{
    public float fallSpeed = 2f;
    public Transform groundCheck;
    public Transform wallCheck;  
    public LayerMask whatIsGround; 
    public float checkDistance = 0.2f; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        if (IsGroundDetected() || IsWallDetected())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Cham roi");
        Player player = collision.GetComponent<Player>();
        //Debug.Log(player);
        if (player != null)
        {
            //Debug.Log("Cham roi ne");
            player.Stats.TakeDamage(30, Color.yellow);
            Destroy(gameObject);
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
}
