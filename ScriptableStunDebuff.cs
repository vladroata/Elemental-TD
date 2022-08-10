using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/StunDebuff")]
public class ScriptableStunDebuff : ScriptableBuff
{
    public float movespeedModifier;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedStunDebuff(this, obj);
    }
}
