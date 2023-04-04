using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnCorpse : MonoBehaviour
{
    public SpriteRenderer corpseObject;
    public float corpseDuration = 5f;

    void Start()
    {
        Destroy(gameObject, corpseDuration);
        
        if (gameObject.tag == "Player") {
            Debug.Log("QUIT");
            Application.Quit();
        }
    }
}
