using TMPro;
using UnityEngine;

public class PopupTextFX : MonoBehaviour
{
    private TextMeshPro _myText;

    [SerializeField] private float speed;
    [SerializeField] private float disappearingSpeed;
    [SerializeField] private float colorDisappearingSpeed;

    [SerializeField] private float lifeTime;

    private float _textTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _myText = GetComponent<TextMeshPro>();
        _textTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _textTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);

        if (_textTimer <= 0)
        {
            float alpha = _myText.color.a - disappearingSpeed * Time.deltaTime;
            _myText.color = new Color(_myText.color.r, _myText.color.g, _myText.color.b, alpha);

            if (_myText.color.a <= 50)
                speed = disappearingSpeed;

            if (_myText.color.a <= 0)
                Destroy(gameObject);
        }
    }
}