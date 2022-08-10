using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedBuff
{
    protected float Duration;
    protected int EffectStacks;
    protected bool IsDamageOverTime;
    protected float DOTtickTimer;
    protected float DamageOverTimeTimer = 0f;
    public ScriptableBuff Buff { get; }
    protected readonly GameObject Obj;
    public bool IsFinished;

    public TimedBuff(ScriptableBuff buff, GameObject obj)
    {
        Buff = buff;
        Obj = obj;
    }

    public void Tick(float delta)
    {
        Duration -= delta;
       
        if(Buff.IsDamageOverTime){
            DamageOverTimeTimer+= delta;
            if(DamageOverTimeTimer >= Buff.DOTtickTimer){
                ApplyEffect();
                DamageOverTimeTimer = 0f;
            }
        }
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    /**
     * Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
     */
    public void Activate()
    {
        if (Buff.IsEffectStacked || Duration <= 0)
        {
            ApplyEffect();
            EffectStacks++;
        }
        
        if (Buff.IsDurationStacked || Duration <= 0)
        {
            Duration += Buff.Duration;
        }
    }
    protected abstract void ApplyEffect();
    public abstract void End();
}