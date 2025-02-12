using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material _originalMat;
    private Coroutine _flashRoutine;
    
    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;

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

        _flashRoutine = null;
    }

    public void Flash()
    {
        if (_flashRoutine is not null)
        {
            StopCoroutine(_flashRoutine);
        }

        _flashRoutine = StartCoroutine(FlashFX());
    }
    
    public void CreateHitFx(Transform target,bool critical)
    {
        float zRotation = 0;
        float xPosition = 0;
        float yPosition = 0;

        Vector3 hitRotation = new Vector3(0,0,zRotation);

        GameObject hitPrefab = hitFx;
        
        // if (critical)
        // {
        //     hitPrefab = criticalFx;
        //
        //     float yRotation = 0;
        //     zRotation = Random.Range(-45, 45);
        //
        //     if (GetComponent<Entity>().FacingDir == -1)
        //         yRotation = 180;
        //
        //     hitFxRotaion = new Vector3(0, yRotation, zRotation);
        //
        // }

        GameObject newHitFx = Instantiate(hitPrefab, target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        newHitFx.transform.Rotate(hitRotation);
        Destroy(newHitFx, .5f);
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