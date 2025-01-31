using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material _originalMat;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originalMat = _spriteRenderer.material;
    }

    private IEnumerator FlashFX()
    {
        _spriteRenderer.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

        _spriteRenderer.material = _originalMat;
    }

    private void RedColorBlink()
    {
        if (_spriteRenderer.color != Color.white)
        {
            _spriteRenderer.color = Color.white;
        }
        else
        {
            _spriteRenderer.color = Color.red;
        }
    }

    private void CancelRedBlink()
    {
        CancelInvoke();

        _spriteRenderer.color = Color.white;
    }
}