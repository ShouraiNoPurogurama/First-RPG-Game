using UnityEngine;
public class FinishPoint : MonoBehaviour
{
    [SerializeField] bool goNextLevel;
    [SerializeField] string levelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Va chạm xảy ra với: " + collision.gameObject.name);

        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Player đã chạm vào FinishPoint!");

            
                //SaveManager.instance.SaveGame();
                SceneController.instance.LoadScene(levelName);
        }
    }
}
