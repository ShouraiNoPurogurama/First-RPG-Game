using System;
using MainCharacter;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;
    private Player _player;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 dir, float gravityScale)
    {
        _rigidbody2D.linearVelocity = dir;
        _rigidbody2D.gravityScale = gravityScale;
    }
}
