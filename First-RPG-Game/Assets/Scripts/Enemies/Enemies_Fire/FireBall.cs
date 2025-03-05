using MainCharacter;
using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    //private Animator anim;
    //[SerializeField] private float fireInterval = 5f;
    //[SerializeField] private float firerDuration = 1f;

    void Start()
    {
        //anim = GetComponent<Animator>();
        //StartCoroutine(WaterColumnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private IEnumerator WaterColumnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            anim.SetBool("isWaterPulse", true);
            yield return new WaitForSeconds(firerDuration);
            anim.SetBool("isWaterPulse", false);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        Debug.Log("aaaaa");
        if (player != null)
        {
            player.Stats.TakeDamage(1,Color.yellow);
        }

    }
}
