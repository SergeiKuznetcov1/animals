using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDmgObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        other.GetComponentInParent<ThirdPersonMovement>().TakeHit(damageAmount: 10);
    }

    private void OnEnable() {
        Invoke(nameof(DeactivateSelf), 0.2f);
    }

    private void DeactivateSelf() {
        gameObject.SetActive(false);
    }
}
