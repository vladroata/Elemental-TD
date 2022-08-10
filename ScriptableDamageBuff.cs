using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/DamageBuff")]
public class ScriptableDamageBuff : ScriptableBuff
{
    public int DamageIncrease;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedDamageBuff(this, obj);
    }
}
