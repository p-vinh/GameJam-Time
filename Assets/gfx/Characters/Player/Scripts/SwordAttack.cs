using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public CharStats stats;
    Vector3 rightAttackOffset;

    private void Start() {
        rightAttackOffset = transform.localPosition;
        stats = GetComponentInParent<CharStats>();
    }

    public void AttackRight() {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void AttackUp() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(0.018f, 0.065f);
    }

    public void AttackDown() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(-0.020f, -0.025f);
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }
    // Bug where player is close to enemy. Giving two hits doesn't register the second hit.
    private void OnTriggerEnter2D(Collider2D collider) {
        IDamageable damageableObject = collider.GetComponentInParent<IDamageable>();

        if(damageableObject != null) {
            // Deal damage to the enemy
            DamageableCharacter enemy = collider.GetComponentInParent<DamageableCharacter>();
            Vector3 parentPosition = transform.parent.position;
            Vector2 direction = (Vector2) (collider.gameObject.GetComponentInParent<Transform>().position - parentPosition).normalized;
            Vector2 knockback = direction * stats.kbForce;
            if (enemy != null) {
                enemy.OnHit(stats.attack, knockback);
            } 
        }
    }
}
