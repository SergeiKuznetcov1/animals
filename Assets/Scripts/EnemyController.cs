using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class EnemyController : MonoBehaviour
{
    public int ExpAmount;
    public GameObject EnemyDmgObject;
    public GameObject AnimalHolder;
    public GameController GameController;
    public int MySpawnPosIndex;
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
    public float InitTimeToNextAttack;
    private float _currentTimeToNextAttack;
    private bool _playerInRange;
    public static Action<int> OnEnemyDeath;
    private void OnEnable() {
        AnimationEvents.OnRespawn += StopAttacking;
    }
    private void OnDisable() {
        AnimationEvents.OnRespawn -= StopAttacking;
    }

    private void StopAttacking()
    {
        _playerInRange = false;
        Animator.ResetTrigger("Attack");
    }

    private void Start() {
        CurrentHealthPoints = MaxHealthPoints;
        HpFillSprite.fillAmount = 1;
        Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update() {
        // Если игрок в рейндже бьём его
        if (_playerInRange == true && _currentTimeToNextAttack <= 0) {
            DoAttackAnim();
            _currentTimeToNextAttack = InitTimeToNextAttack;
        } 
        else if (_playerInRange) {
            _currentTimeToNextAttack -= Time.deltaTime;
        }
        Canvas.LookAt(Camera);
    }

    private void OnTriggerStay(Collider other) {
        // Поворачиваемся к игроку если он в рейндже
        if (other.gameObject.CompareTag("Player")) {
            _playerInRange = true;
            Vector3 target = other.transform.position - EnemyParentObject.position;
            target.y = 0;
            Quaternion lookDir = Quaternion.LookRotation(target, Vector3.up);
            EnemyParentObject.rotation = Quaternion.Slerp(EnemyParentObject.rotation, lookDir,
                TurnSpeed * Time.deltaTime);
            _currentTimeToNextAttack = 0;
        }  
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            _playerInRange = false;
            Animator.ResetTrigger("Attack");
        }
    }

    public void DoAttackAnim() {
        Animator.SetTrigger("Attack");
    }

    public void ActivateEnemyDmgObject() {
        EnemyDmgObject.SetActive(true);
    }

    public void TakeHit(int dmg) {
        if (_dead == true) {
            return;
        }
        CurrentHealthPoints -= dmg;
        CurrentHealthPoints = Mathf.Clamp(CurrentHealthPoints, 0, MaxHealthPoints);
        if (CurrentHealthPoints == 0) {
            Animator.SetBool("Dead", true);
            OnEnemyDeath?.Invoke(ExpAmount);
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

    public void ResetAnimal() {
        HpFillSprite.fillAmount = 1;
        CurrentHealthPoints = MaxHealthPoints;
        HpText.text = $"{CurrentHealthPoints} / 100";
        Animator.SetBool("Dead", false);
        InnerCollider.enabled = true;
        MyCollider.enabled = true;
        _dead = false;
        Canvas.gameObject.SetActive(true);
        AnimalHolder.SetActive(true);
    }

    public void HideAnimal() {
        GameController.StartCountdownToRes(MySpawnPosIndex, gameObject);
        AnimalHolder.SetActive(false);
    }
}
