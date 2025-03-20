using Stats;
using UnityEngine;

public class ExplosiveController : MonoBehaviour
{
    private Animator _animator;
    private CharacterStats _myStats;
    private float _growSpeed = 15;
    private float _maxSize = 6;
    private float _explosionRadius;

    private bool _canGrow = true;

    private void Update()
    {
        if (_canGrow)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);
        }

        if (_maxSize - transform.localScale.x < .5f)
        {
            _canGrow = false;
            _animator.SetTrigger("Explode");
        }
    }

    public void SetupExplosive(CharacterStats mystats, float growSpeed, float maxSize, float explosionRadius)
    {
        _animator = GetComponent<Animator>();
        
        _myStats = mystats;
        _growSpeed = growSpeed;
        _maxSize = maxSize;
        _explosionRadius = explosionRadius;
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        foreach (var hit in colliders)
        {
            var target = hit.GetComponent<Entity>();

            if (target)
            {
                target.SetupKnockBackDir(transform);
                _myStats.DoDamage(target.GetComponent<CharacterStats>());
            }
        }
    }
    
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}