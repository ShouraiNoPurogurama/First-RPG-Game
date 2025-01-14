using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine StateMachine;
    protected Player Player;

    protected Rigidbody2D Rb;

    protected float xInput;
    private readonly string _animationBoolName;

    protected float stateTimer;

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animationBoolName)
    {
        StateMachine = stateMachine;
        Player = player;
        _animationBoolName = animationBoolName;
    }

    public virtual void Enter()
    {
        Player.Animator.SetBool(_animationBoolName, true);
        Rb = Player.Rb;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        
        xInput = Input.GetAxisRaw("Horizontal");
        
        Player.Animator.SetFloat("yVelocity", Rb.linearVelocity.y);
    }

    public virtual void Exit()
    {
        Player.Animator.SetBool(_animationBoolName, false);
    }
}