using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDmgObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        other.GetComponentInParent<EnemyController>().TakeHit(50);
    }

    private void OnEnable() {
        Invoke(nameof(DeactivateSelf), 0.2f);
    }

    private void DeactivateSelf() {
        gameObject.SetActive(false);
    }
}
