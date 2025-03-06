using UnityEngine;

public class ParallaxBackgroundMultiLayers : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private float parallaxEffect;

    private float _startXPosition;
    private float _length;
    private float _startCameraXPosition;
    
    void Start()
    {
        _camera = Camera.main;

        _length = 76f; 
        _startXPosition = transform.position.x;
        _startCameraXPosition = _camera.transform.position.x;
    }

    void Update()
    {
        var cameraDeltaX = _camera.transform.position.x - _startCameraXPosition;
        var distanceToMove = cameraDeltaX * parallaxEffect;

        transform.position = new Vector3(_startXPosition + distanceToMove, transform.position.y, transform.position.z);

        var cameraPositionX = _camera.transform.position.x;
        var backgroundEdgeRight = _startXPosition + _length / 2;
        var backgroundEdgeLeft = _startXPosition - _length / 2;

        if (cameraPositionX > backgroundEdgeRight && distanceToMove != 0)
        {
            _startXPosition += _length;
            _startCameraXPosition += _length;
        }
        else if (cameraPositionX < backgroundEdgeLeft && distanceToMove != 0)
        {
            _startXPosition -= 0.9f * _length;
            _startCameraXPosition -= 0.9f * _length;
        }
    }
}