using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public static Action OnAttackActivation;
    public static Action OnAttackDeactivation;
    public static Action OnHealEnd;
    public static Action OnRespawn;
    public void InvokeAttackActivation() {
        OnAttackActivation?.Invoke();
    }
    public void InvokeAttackDeactivation() {
        OnAttackDeactivation?.Invoke();
    }
    public void InvokeHealEnd() {
        OnHealEnd?.Invoke();
    }
    public void InvokeRespawn() {
        OnRespawn?.Invoke();
    }
}
