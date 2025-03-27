using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SkeletonSeekerStunnedState : EnemyState
{
    private SkeletonSeeker _skeletonSeeker;
    public SkeletonSeekerStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName)
    {
        _skeletonSeeker = skeletonSeeker;
    }
}
