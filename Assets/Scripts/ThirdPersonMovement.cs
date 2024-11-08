using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Health Points")]
    public Image HealthFill;
    public TMP_Text CurrentHP;
    public int MaxHealth;
    private int _currentHealth;
    [Header("Experience")]
    private int _currentLevel;
    public Image ExperienceFill;
    public TMP_Text ExperienceText;
    private int _maxExp;
    private int _currentExp;
    [Space]
    public CharacterController CharacterController;
    public Transform Cam;
    public Transform DmgObject;
    public Animator Animator;
    public static bool IsAttacking;
    public float MoveSpeed;
    public float JumpSpeed;
    public float _ySpeed;
    public float RotationSpeed;
    private bool _playerDead;
    private bool _healing;
    private void OnEnable() {
        AnimationEvents.OnAttackActivation += ActivateDmgObject;
        AnimationEvents.OnAttackDeactivation += DeactivateAttacking;
        AnimationEvents.OnHealEnd += EndHeal;
        AnimationEvents.OnRespawn += RespawnPlayer;
        EnemyController.OnEnemyDeath += AddExp;
    }

    private void AddExp(int exp)
    {
        _currentExp += exp;
        if (_currentExp >= _maxExp) {
            IncreaseLevel();
        }
        ExperienceText.text = $"{_currentExp} / {_maxExp}";
        PlayerPrefs.SetInt("Exp", _currentExp);
        StopCoroutine(nameof(IncreaseFill));
        StartCoroutine(nameof(IncreaseFill));
    }

    private void IncreaseLevel()
    {
        _currentLevel += 1;
        PlayerPrefs.SetInt("Level", _currentLevel);
        MaxHealth = 100 + 5 * _currentLevel;
        _currentHealth = MaxHealth;
        HealthFill.fillAmount = 1;
        CurrentHP.text = $"{_currentHealth} / {MaxHealth}";
        _maxExp = 100 + 5 * _currentLevel;
    }

    private void OnDisable() {
        AnimationEvents.OnAttackActivation -= ActivateDmgObject;
        AnimationEvents.OnAttackDeactivation -= DeactivateAttacking;
        AnimationEvents.OnHealEnd -= EndHeal;
        AnimationEvents.OnRespawn -= RespawnPlayer;
        EnemyController.OnEnemyDeath -= AddExp;
    }
    private void Start() {
        _currentExp = PlayerPrefs.GetInt("Exp", 0);
        _currentLevel = PlayerPrefs.GetInt("Level", 0);
        MaxHealth = 100 + 5 * _currentLevel;
        _maxExp = 100 + 100 * _currentLevel;
        ExperienceFill.fillAmount = (float)_currentExp / (float)_maxExp;
        _currentHealth = MaxHealth;
        ExperienceText.text = $"{_currentExp} / {_maxExp}";
        CurrentHP.text = $"{_currentHealth} / {MaxHealth}";
    }
    // Update is called once per frame
    void Update()
    {
        if (_healing == true) {
            return;
        }
        if (_playerDead == true) {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        _ySpeed += Physics.gravity.y * Time.deltaTime;
        direction = Quaternion.AngleAxis(Cam.rotation.eulerAngles.y, Vector3.up) * direction;
        if (CharacterController.isGrounded) {
            _ySpeed = -0.5f;
            if (Input.GetButtonDown("Jump")) {
                _ySpeed = JumpSpeed;
            }
        }

        Vector3 velocity = direction * MoveSpeed;
        velocity.y = _ySpeed;
        CharacterController.Move(velocity * Time.deltaTime);
        if (direction.magnitude >= 0.1f) {
            Quaternion lookDir = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * RotationSpeed);
            if (IsAttacking == false) {
                Animator.SetBool("isWalking", true);
            }
        }
        else {
            if (IsAttacking == false) {
                Animator.SetBool("isWalking", false);
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            IsAttacking = true;
            Animator.SetBool("isAttacking", true);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            if (_healing == false) {
                Animator.SetTrigger("Heal");
                _healing = true;
            }
        }
    }
    private void ActivateDmgObject() {
        DmgObject.gameObject.SetActive(true);
    }
    private void DeactivateAttacking() {
        IsAttacking = false;
        Animator.SetBool("isAttacking", false);
    }
    private void EndHeal() {
        _healing = false;
        _currentHealth = MaxHealth;
        CurrentHP.text = $"{_currentHealth} / {MaxHealth}";
    }
    private void RespawnPlayer() {
        _playerDead = false;
        _currentHealth = MaxHealth;
        CurrentHP.text = $"{_currentHealth} / {MaxHealth}";
        Animator.SetBool("isDead", false);
        HealthFill.fillAmount = 1;
    }

    private void OnApplicationFocus(bool focusStatus) {
        if (focusStatus) {
            Cursor.lockState = CursorLockMode.Locked;
        }    
        else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void TakeHit(int damageAmount) {
        if (_playerDead == false && _healing == false) {
            _currentHealth -= damageAmount;
            CurrentHP.text = $"{_currentHealth} / {MaxHealth}";
            if (_currentHealth <= 0) {
                _playerDead = true;
                Animator.SetBool("isDead", true);
            }
        }
        StopCoroutine(nameof(ReduceFill));
        StartCoroutine(nameof(ReduceFill));
    }
    private IEnumerator ReduceFill() {
        while (HealthFill.fillAmount > (float)_currentHealth / (float)MaxHealth) {
            HealthFill.fillAmount -= Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator IncreaseFill() {
        while (ExperienceFill.fillAmount < (float)_currentExp / (float)_maxExp) {
            ExperienceFill.fillAmount += Time.deltaTime;
            yield return null;
        }
    }
}
