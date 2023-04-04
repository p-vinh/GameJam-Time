using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moved to enemy class
// Makes an object move randomly in a set direction
public class Wander : MonoBehaviour
{
    private Animator animator;
    private float decisionTimeCount = 0;
    private Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.zero, Vector3.zero, new Vector3(.5f,.5f), new Vector3(-.5f,.5f), new Vector3(.5f,-.5f), new Vector3(-.5f,-.5f)};
    private int currentMove;
    CharStats stats;
    public Vector2 decisionTime = new Vector2(1, 4);
    private Enemy enemy;
    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponentInParent<CharStats>();
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
        ChooseMoveDirection();
    }

// should probably move to enemy
    private void FixedUpdate() {
        if (enemy.currState != Enemy.State.attack) {
            if (moveDirections[currentMove] == Vector3.zero) {
                animator.SetBool("wander", false);
                enemy.currState = Enemy.State.idle;
            } else {
                rb.AddForce(moveDirections[currentMove] * Time.deltaTime * stats.speed);
                animator.SetBool("wander", true);
                enemy.currState = Enemy.State.wandering;
            }

            if (decisionTimeCount > 0) decisionTimeCount -= Time.deltaTime;
            else {
                decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
                ChooseMoveDirection();
            }
        }
    }

    private void ChooseMoveDirection() {
        currentMove = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
    }

}