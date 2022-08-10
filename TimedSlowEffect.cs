using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSlowEffect : TimedBuff
{
    enemyInterface enemyInterface;

    public TimedSlowEffect(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        enemyInterface = obj.GetComponent<enemyInterface>();
    }

    protected override void ApplyEffect()
    {
        //Add Damage increase to MovementComponent
        ScriptableSlowEffect slowDebuff = (ScriptableSlowEffect) Buff;
        enemyInterface.setSpeed(enemyInterface.getSpeed()*(1.0f - slowDebuff.SlowAmount));
    }

    public override void End()
    {
        //Revert speed increase
        ScriptableSlowEffect slowDebuff= (ScriptableSlowEffect) Buff;
        enemyInterface.setSpeed(enemyInterface.getSpeed()/(1.0f - slowDebuff.SlowAmount));
        EffectStacks = 0;
    }
}