using UnityEngine;

public interface IDamageable {
    public float Health {set; get;}
    public bool Invincible { set; get; }
    void OnHit(float damge, Vector2 knockback);

    void OnHit(float damge);
}