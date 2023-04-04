using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageableCharacter : MonoBehaviour, IDamageable {

    Animator animator;
    CharStats stats;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    GameObject health;
    [SerializeField] Color originalColor;
    [SerializeField] Collider2D physicsCollider;
    [SerializeField] GameObject healthText;
    [SerializeField] GameObject corpsePrefab;
    TimeController controller;
    float invincibilityTimeElapsed = 0f;
    public float invincibilityTime = 0.25f;
    public bool isInvincibileEnabled = false;

    public void Start() {
        controller = FindAnyObjectByType<TimeController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stats = GetComponent<CharStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void FixedUpdate() {
        if (Invincible) {
            invincibilityTimeElapsed += Time.deltaTime;
            
            if (invincibilityTimeElapsed > invincibilityTime) {
                Invincible = false;
            }
        }
    }

    public float Health {
        set {
            if (value < stats.health) {
                animator.SetTrigger("attacked");
                spriteRenderer.color = Color.red;
                health = Instantiate(healthText);
                RectTransform textTransform = health.GetComponent<RectTransform>();
                textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.GetComponent<Transform>().position);
                
                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                textTransform.SetParent(canvas.transform);
            }

            stats.health = value;
            if (stats.health <= 0) {
                Defeated();
            } else {
                Invoke("ResetColor", 0.1f); // Reset color after 0.1 seconds
            }
        }
        get {
            return stats.health;
        }
    }

    public bool Invincible { 
        get {
            return _invincible;
        }
    
        set {
            _invincible = value;
            
            if (_invincible == true) {
                invincibilityTimeElapsed = 0f;
            }
        }
    }

    public bool _invincible = false;

    public void OnHit(float damage, Vector2 knockback) {
        if (!Invincible) {
            this.Health -= damage;
            displayDamage(damage);
            rb.AddForce(knockback);

            if (isInvincibileEnabled) {
                Invincible = true;
            }
        }
    }

    public void OnHit(float damage) {
        if (!Invincible) {
            this.Health -= damage;
            displayDamage(damage);

            if (isInvincibileEnabled) {
                Invincible = true;
            }
        }
    }

    private void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }

    public void Defeated() {
        physicsCollider.enabled = false;
        animator.SetTrigger("Defeated");
    }
    public void createCorpse() {
        Instantiate(corpsePrefab, transform.position, Quaternion.identity);
    }
    public void RemoveEnemy() {
        controller.RemoveObject(gameObject);
        Destroy(gameObject);
    }

    private void displayDamage(float damage) {
        TextMeshProUGUI txt = health.GetComponent<TextMeshProUGUI>();
        txt.text = damage.ToString();
    }
}