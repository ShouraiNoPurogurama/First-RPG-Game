using Skills;
using UnityEngine;

namespace MainCharacter
{
    public class PlayerTeleportSwordState : PlayerState
    {
        private TeleportSwordSkill _teleportSkill;

        public PlayerTeleportSwordState(PlayerStateMachine stateMachine, Player player, string animationBoolName)
            : base(stateMachine, player, animationBoolName)
        {
            _teleportSkill = player.GetComponent<TeleportSwordSkill>();
        }

        public override void Enter()
        {
            base.Enter();
            _teleportSkill.ThrowTeleportSword(); // Ném kiếm ngay khi vào trạng thái
            StateMachine.ChangeState(Player.IdleState); // Quay lại trạng thái Idle ngay lập tức
        }

        public override void Update()
        {
            base.Update();
            Player.SetZeroVelocity(); // Dừng di chuyển trong lúc ném
        }

        public override void Exit()
        {
            base.Exit();
            Player.StartCoroutine("BusyFor", 0.15f); // Giữ nhân vật bận trong 0.15 giây
        }
    }
}