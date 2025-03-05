using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;


    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material _originalMat;
    private Coroutine _flashRoutine;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;

    [Header("Ailment FX")]
    [SerializeField] private Color[] chillColor;

    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Popup FX")]
    [SerializeField] private GameObject popupFxPrefab;
    
    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originalMat = _spriteRenderer.material;
    }

    public void CreatePopUpText(string _text, Color? color)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1.5f, 3);

        Vector3 positionOffset = new Vector3(randomX, randomY);
        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        TextMeshPro textMesh = newText.GetComponent<TextMeshPro>();
        textMesh.text = _text;
        textMesh.color = color ?? Color.white;
    }


    private IEnumerator FlashFX()
    {
        _spriteRenderer.material = hitMat;
        
        var currentColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.color = currentColor;
        
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

    public void CreateHitFx(Transform target, bool critical)
    {
        float zRotation = 0;
        float xPosition = .5f;
        float yPosition = 0;

        Vector3 hitRotation = new Vector3(0, 0, zRotation);

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

    private void CancelColorChange()
    {
        CancelInvoke();
        _spriteRenderer.color = Color.white;
    }

    public void IgniteFxFor(float seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", seconds);
    }

    private void IgniteColorFX()
    {
        if (_spriteRenderer.color != igniteColor[0])
        {
            _spriteRenderer.color = igniteColor[0];
        }
        else
        {
            _spriteRenderer.color = igniteColor[1];
        }
    }

    public void ChillFxFor(float seconds)
    {
        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", seconds);
    }

    private void ChillColorFX()
    {
        if (_spriteRenderer.color != chillColor[0])
        {
            _spriteRenderer.color = chillColor[0];
        } else
        {
            _spriteRenderer.color = chillColor[1];
        }
    }

    public void ShockFxFor(float seconds)
    {
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", seconds);
    }
    
    private void ShockColorFX()
    {
        if (_spriteRenderer.color != shockColor[0])
        {
            _spriteRenderer.color = shockColor[0];
        }
        else
        {
            _spriteRenderer.color = shockColor[1];
        }
    }
}