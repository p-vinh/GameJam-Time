using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SignText : MonoBehaviour
{
    public AnimatedText text;
    public string displayText;
    public TextMeshProUGUI textUI;


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            text.AnimateText(0.4f, displayText);
        }
    }


}
