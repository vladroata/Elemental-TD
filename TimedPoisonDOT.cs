using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPoisonDOT : TimedBuff
{
    enemyInterface enemyInterface;
    int TickDamage = 20;

    public TimedPoisonDOT(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        enemyInterface = obj.GetComponent<enemyInterface>();
    }

    protected override void ApplyEffect()
    {
        //Add Damage increase to MovementComponent
        ScriptablePoisonDOT poison = (ScriptablePoisonDOT) Buff;
        enemyInterface.setHealth(enemyInterface.getHealth()-poison.TickDamage);
    }

    public override void End()
    {
        //No effect on end.
        EffectStacks = 0;
    }
}
