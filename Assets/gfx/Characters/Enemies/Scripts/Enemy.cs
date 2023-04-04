using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : TimeControlled
{
    public enum State {
        idle,
        wandering,
        attack,
    }
    public State currState = State.idle;
    public CharStats stats;
    public FollowCollider detectionArea;
    Vector3[] moveDirections = new Vector3[] {
        Vector3.right, Vector3.left, Vector3.up, 
        Vector3.down, Vector3.zero, Vector3.zero, 
        new Vector3(.5f,.5f), new Vector3(-.5f,.5f), 
        new Vector3(.5f,-.5f), new Vector3(-.5f,-.5f)};
    Vector2 decisionTime = new Vector2(1, 4);
    int currentMove;
    float decisionTimeCount = 0;
    Animator animator;
    Rigidbody2D rb;

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
        ChooseMoveDirection();
    }
    private void Update() {
        if(detectionArea.detectedObj.Count > 0) {
            currState = State.attack;
            // This may need to cahnge so that it can be modular for other enemies.
            // Have slime class do their special attack.
            Vector2 direction = (detectionArea.detectedObj[0].transform.position - transform.position).normalized;
            rb.AddForce(direction * stats.speed * Time.deltaTime);
        }
        else
            currState = State.wandering;


        if (currState == State.wandering) {
            if (moveDirections[currentMove] == Vector3.zero) {
                animator.SetBool("wander", false);
                currState = State.idle;
            } else {
                rb.AddForce(moveDirections[currentMove] * Time.deltaTime * stats.speed);
                animator.SetBool("wander", true);
                currState = State.wandering;
            }

            if (decisionTimeCount > 0) decisionTimeCount -= Time.deltaTime;
            else {
                decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
                ChooseMoveDirection();
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        IDamageable damageable = other.collider.GetComponent<IDamageable>();
        
        if (damageable != null && other.gameObject.CompareTag("Player")) {
            Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2) (other.gameObject.GetComponentInParent<Transform>().position - parentPosition).normalized;
            Vector2 knockback = direction * stats.kbForce;
            damageable.OnHit(stats.attack, knockback);
        }
    }
    
    private void ChooseMoveDirection() {
        currentMove = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
    }
}
