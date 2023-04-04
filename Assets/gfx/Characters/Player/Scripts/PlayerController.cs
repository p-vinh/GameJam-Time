using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Takes and handles input and movement for a player character
public class PlayerController : TimeControlled
{
    [SerializeField] CharStats stats;
    [SerializeField] HealthBar healthBar;

    [SerializeField] private float currentStamina;
    public float maxStamina = 100;
    float timeSinceStopped = 0f;
    float fillSpeedMultiplier = 1f;
    public float fillSpeedIncreaseTime = 2f;
    public float fillSpeed = 1f; // Speed at which the bar fills
    [SerializeField] HealthBar staminaBar;
    
    [SerializeField] SwordAttack swordAttack;
    [SerializeField] Vector2 movementInput;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    private bool canMove = true;
    private bool isMoving = false;
    private int side = 0; // 0 up, 1 down, 2 left, 3 right


    bool IsMoving {
        set {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    int Side {
        set {
            side = value;
            animator.SetInteger("side", side);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxHealth(maxStamina);
        healthBar.SetMaxHealth(stats.health);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordAttack = GetComponentInChildren<SwordAttack>();
    }

    public override void TimeUpdate() {
        healthBar.SetHealth(stats.health);
        // Recovering Time Energy


        // If movement input is not 0, try to move
        if(canMove && movementInput != Vector2.zero){
            UpdateAnimation();
            if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0) {
                // Decreasing time energy
                currentStamina -= Time.deltaTime * 20;
                staminaBar.SetHealth(currentStamina);
                rb.velocity += movementInput * stats.speed * 2 * Time.deltaTime;  
            }
            else {
                rb.velocity += movementInput * stats.speed * Time.deltaTime;
            }
            timeSinceStopped = 0f;
        } else {
            IsMoving = false;
            timeSinceStopped += Time.deltaTime;
            fillSpeedMultiplier = 1f + (timeSinceStopped / fillSpeedIncreaseTime);
            currentStamina += Time.deltaTime * fillSpeed * fillSpeedMultiplier;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            staminaBar.SetHealth(currentStamina);
        }
    }

    private void UpdateAnimation() {
        IsMoving = true;
        if (Input.GetKey("w") && movementInput.y > 0) {
            Side = 0;
        }
        if (Input.GetKey("s") && movementInput.y < 0) {
            Side = 1;
        }
        // Set direction of sprite to movement direction
        if(Input.GetKey("a") && movementInput.x < 0) {
            spriteRenderer.flipX = true;
            Side = 2;
        }
        if (Input.GetKey("d") && movementInput.x > 0) {
            spriteRenderer.flipX = false;
            Side = 3;
        }   
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire() {
        animator.SetTrigger("swordAtk");
    }

    public void SwordAttack() {
        LockMovement();
        
        switch (side) {
            case 0:
                swordAttack.AttackUp();
                break;
            case 1:
                swordAttack.AttackDown();
                break;
            case 2:
                swordAttack.AttackLeft();
                break;
            case 3:
                swordAttack.AttackRight();
                break;
        }
    }

    public void EndSwordAttack() {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }

}
