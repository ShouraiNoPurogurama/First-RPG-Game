using Enemies;
using UnityEngine;

public class BossSkeletonKnightDisappearState : EnemyState
{
    private BossSkeletonKnight enemy;

    public BossSkeletonKnightDisappearState(
        Enemy enemyBase,
        EnemyStateMachine stateMachine,
        string animBoolName,
        BossSkeletonKnight enemy
    ) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (TriggerCalled)
        {
            TeleportBoss();
            StateMachine.ChangeState(enemy.AppearState);
        }
    }

    private void TeleportBoss()
    {
        var player = GameObject.FindAnyObjectByType<MainCharacter.Player>();
        if (player == null) return;

        // 1) Xác định hướng sau lưng player
        // Giả sử player.transform.localScale.x > 0 => Player đang quay mặt sang phải => sau lưng là Vector2.left
        float facingX = player.transform.localScale.x;
        Vector2 behindDir = (facingX > 0) ? Vector2.left : Vector2.right;

        float behindDistance = 3f;

        LayerMask whatIsWall = LayerMask.GetMask("Ground", "Wall");
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, behindDir, behindDistance, whatIsWall);

        if (hit.collider != null)
        {
            // Nếu trúng tường, ta giảm khoảng cách lại để boss không bị “lọt” vào tường
            // hit.distance là khoảng cách từ player đến tường
            behindDistance = hit.distance - 0.3f;
            if (behindDistance < 0f) behindDistance = 0f;
        }

        // 4) Tính toạ độ cuối cùng sau lưng player
        Vector2 playerPos = player.transform.position;
        Vector2 behindPos = playerPos + behindDir * behindDistance;

        enemy.transform.position = new Vector3(
            behindPos.x,
            behindPos.y,
            enemy.transform.position.z
        );
    }

}
