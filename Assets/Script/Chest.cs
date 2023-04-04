using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject open;
    public GameObject close;
    public SignText text;
    private string output = "";
    public CharStats playerStats;

    private void randomStats() {
        output += "Health: ";
        playerStats.health += randomNumber();
        output += "Attack: ";
        playerStats.attack += randomNumber();
        output += "Knockback: ";
        playerStats.kbForce += randomNumber();
        output += "Speed: ";
        playerStats.speed += randomNumber();
    }

    private float randomNumber() {
        int randomNumber = Random.Range(1, 50);
        output += randomNumber + "\n";
        return randomNumber;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            randomStats();
            text.displayText = output;
            output = "";
            close.SetActive(false);
            open.SetActive(true);
        }
    }
}
