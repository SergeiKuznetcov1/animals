using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyController : MonoBehaviour
{
    public int MySpawnPosIndex;
    public EnemyType EnemyType;
    public Animator Animator;
    public SphereCollider InnerCollider;
    public SphereCollider MyCollider;
    public float MaxHealthPoints;
    public float CurrentHealthPoints;
    public Image HpFillSprite;
    public TMP_Text HpText;
    private Transform Camera;
    public Transform Canvas;
    public Transform EnemyParentObject;
    public float TurnSpeed;
    private bool _dead;
    private void Start() {
        CurrentHealthPoints = MaxHealthPoints;
        HpFillSprite.fillAmount = 1;
        Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Vector3 target = other.transform.position - EnemyParentObject.position;
            target.y = 0;
            Quaternion lookDir = Quaternion.LookRotation(target, Vector3.up);
            EnemyParentObject.rotation = Quaternion.Slerp(EnemyParentObject.rotation, lookDir,
                TurnSpeed * Time.deltaTime);
        }  
    }

    public void TakeHit(int dmg) {
        if (_dead == true) {
            return;
        }
        CurrentHealthPoints -= dmg;
        CurrentHealthPoints = Mathf.Clamp(CurrentHealthPoints, 0, MaxHealthPoints);
        if (CurrentHealthPoints == 0) {
            Animator.SetBool("Dead", true);
            InnerCollider.enabled = false;
            MyCollider.enabled = false;
            _dead = true;
            Canvas.gameObject.SetActive(false);
        }
        HpText.text = $"{CurrentHealthPoints} / 100";
        StopCoroutine(nameof(ReduceFill));
        StartCoroutine(nameof(ReduceFill));
    }

    private IEnumerator ReduceFill() {
        while (HpFillSprite.fillAmount > CurrentHealthPoints / MaxHealthPoints) {
            HpFillSprite.fillAmount -= Time.deltaTime;
            yield return null;
        }
    }

    private void Update() {
        Canvas.LookAt(Camera);
    }
}
