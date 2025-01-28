using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }

    public EnemyStateMachine StateMachine { get; private set; }

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
    }
}