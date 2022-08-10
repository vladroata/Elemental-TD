using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/PoisonDOT")]
public class ScriptablePoisonDOT : ScriptableBuff
{
    public int TickDamage;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedPoisonDOT(this, obj);
    }
}
