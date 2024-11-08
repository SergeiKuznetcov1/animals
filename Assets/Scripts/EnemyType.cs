using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public EnumEnemyType MyType;
    public EnemyController MyEnemyController;
    // Вызывается в конце анимации смерти
    public void InvokeHide() {
        MyEnemyController.HideAnimal();
    }
    // Вызывается в середине атаки анимации
    public void InvokeDmgObject() {
        MyEnemyController.ActivateEnemyDmgObject();
    }
}
