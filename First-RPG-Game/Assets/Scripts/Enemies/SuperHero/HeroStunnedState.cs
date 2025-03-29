using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HeroStunnedState : EnemyState
{
    private Hero _Hero;
    public HeroStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hero Hero) : base(enemyBase, stateMachine, animBoolName)
    {
        _Hero = Hero;
    }
}
