using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int _comboCounter;

    private float _lastTimeAttacked;
    private float _comboWindow = 2;

    public PlayerPrimaryAttack(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine,
        player, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
        {
            _comboCounter = 0;
        }

        Player.Animator.SetInteger("ComboCounter", _comboCounter);
        
    }

    public override void Update()
    {
        base.Update();

        //Change state when player trigger is called (animation ends)
        if (TriggerCalled)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _comboCounter++;
        _lastTimeAttacked = Time.time;
    }
}