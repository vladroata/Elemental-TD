using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedReverseDebuff : TimedBuff
{
    enemyInterface enemyInterface;
    bool effectFired;

    public TimedReverseDebuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        enemyInterface = obj.GetComponent<enemyInterface>();
    }

    protected override void ApplyEffect()
    {
        int random = UnityEngine.Random.Range(0, 100);
        ScriptableReverseDebuff reverseDebuff = (ScriptableReverseDebuff) Buff;
        if(random <= reverseDebuff.chanceToActivate){
            effectFired = true;
            enemyInterface.walkReverse();
        }
    }

    public override void End()
    {
        if(effectFired == true){
            enemyInterface.walkReverse();
        }
        EffectStacks = 0;
    }
}