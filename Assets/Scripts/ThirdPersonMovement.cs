using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public int MaxHealth;
    private int _currentHealth;
    public CharacterController CharacterController;
    public Transform Cam;
    public Transform DmgObject;
    public Animator Animator;
    public static bool IsAttacking;
    public bool DontRunUpdate;
    public float MoveSpeed;
    public float JumpSpeed;
    public float _ySpeed;
    public float RotationSpeed;
    private bool _playerDead;
    private void Start() {
        _currentHealth = MaxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (DontRunUpdate == true) {
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDir, Time.deltaTime * RotationSpeed);
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
    }

    public void DeactivateAttacking() {
        IsAttacking = false;
        Animator.SetBool("isAttacking", false);
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
        if (_playerDead == false) {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0) {
                _playerDead = true;
                Debug.Log("Player dead");
            }
        }
        Debug.Log(_currentHealth);
    }

    public void ActivateDmgObject() {
        DmgObject.gameObject.SetActive(true);
    }
}
