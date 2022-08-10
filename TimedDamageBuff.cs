using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDamageBuff : TimedBuff
{
    myInterface tower_interface;

    public TimedDamageBuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        tower_interface = obj.GetComponent<myInterface>();
    }

    protected override void ApplyEffect()
    {
        //Add Damage increase to MovementComponent
        ScriptableDamageBuff damageBuff = (ScriptableDamageBuff) Buff;
        tower_interface.setDamage(tower_interface.getDamage()*damageBuff.DamageIncrease);
    }

    public override void End()
    {
        //Revert speed increase
        ScriptableDamageBuff damageBuff = (ScriptableDamageBuff) Buff;
        tower_interface.setDamage(tower_interface.getBaseDamage());
        EffectStacks = 0;
    }
}