using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/ReverseDebuff")]
public class ScriptableReverseDebuff : ScriptableBuff
{
    public int chanceToActivate;
    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedReverseDebuff(this, obj);
    }
}
