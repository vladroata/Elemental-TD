using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SlowDeBuff")]
public class ScriptableSlowEffect : ScriptableBuff
{
    public float SlowAmount; //Example SlowAmount = 0.2 --> 20% slow

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedSlowEffect(this, obj);
    }
}
