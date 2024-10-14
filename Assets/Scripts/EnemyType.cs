using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public EnumEnemyType MyType;
    public EnemyController MyEnemyController;
    public void InvokeHide() {
        MyEnemyController.HideAnimal();
    }
    public void InvokeDmgObject() {
        MyEnemyController.ActivateEnemyDmgObject();
    }
}
