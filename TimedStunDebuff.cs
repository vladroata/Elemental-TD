using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedStunDebuff : TimedBuff
{
    enemyInterface enemyInterface;
    float defaultSpeed;

    public TimedStunDebuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        enemyInterface = obj.GetComponent<enemyInterface>();
        defaultSpeed = enemyInterface.getSpeed();
    }

    protected override void ApplyEffect()
    {
        //Add Damage increase to MovementComponent
        ScriptableStunDebuff stunDebuff = (ScriptableStunDebuff) Buff;
        enemyInterface.setSpeed(stunDebuff.movespeedModifier);
    }

    public override void End()
    {
        //Revert speed increase
        ScriptableStunDebuff damageBuff = (ScriptableStunDebuff) Buff;
        enemyInterface.setSpeed(defaultSpeed);
        EffectStacks = 0;
    }
}