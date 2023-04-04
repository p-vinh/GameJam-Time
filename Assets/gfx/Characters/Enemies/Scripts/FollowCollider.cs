using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCollider : MonoBehaviour
{
    public Animator animator;
    public List<Collider2D> detectedObj = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            detectedObj.Add(other);
            Following(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            detectedObj.Remove(other);
            Following(false);
        }
    }  
    public void Following(bool isFollowing) {
        animator.SetBool("Following", isFollowing);
    }
}
