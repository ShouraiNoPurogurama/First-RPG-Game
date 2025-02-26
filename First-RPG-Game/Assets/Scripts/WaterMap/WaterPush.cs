using UnityEngine;

public class WaterPush : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private SpriteRenderer spriteRend;
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRend && spriteRend.sprite != null)
        {
            Vector2 spriteSize = spriteRend.sprite.bounds.size;

            Vector3 totalScale = spriteRend.transform.lossyScale;

            spriteSize.x *= totalScale.x;
            spriteSize.y *= totalScale.y;

            boxCol.size = spriteSize;
        }
    }
}
