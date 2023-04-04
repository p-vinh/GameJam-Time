using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatsMenu : MonoBehaviour
{
    public CharStats stats;
    public TextMeshProUGUI display;

    private void Start() {
        StartCoroutine(UpdateStats());
    }

    private IEnumerator UpdateStats() {
        yield return new WaitForSeconds(1);
        display.text = "Health: " + stats.health + "\n"
                      + "Attack: " + stats.attack + "\n"
                      + "KnockBack: " + stats.kbForce + "\n"
                      + "Speed: " + stats.speed + "\n";
        StartCoroutine(UpdateStats());
    }
}
