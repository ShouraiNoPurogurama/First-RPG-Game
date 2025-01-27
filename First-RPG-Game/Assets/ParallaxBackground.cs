using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject _camera;
    [SerializeField] private float parallaxEffect;

    private float _xPosition;
    private float _length;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = GameObject.Find("Main Camera");

        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // The background move gap after applying parallaxEffect compared to original speed
        var distanceMoved = _camera.transform.position.x * (1 - parallaxEffect);
        var distanceToMove = _camera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(_xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > _xPosition + _length)
        {
            _xPosition += _length;
        }
        else if (distanceMoved < _xPosition - _length)
        {
            _xPosition -= _length;
        }
    }
}