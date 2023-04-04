using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimatedText : MonoBehaviour
{
      [Header("UI")]
      [SerializeField] private TextMeshProUGUI displayedTextMesh;
      [SerializeField] private float delayBetweenWord = 0.1f;            

      private string textToAnimate;
      private string currentText;        

      private string[] splitMessage;
      private int indexWord = 0;
      private string previousWord;

      public void AnimateText(float startDelay, string text)
      {
            StopAllCoroutines();
            indexWord = 0;
            textToAnimate = text;

            displayedTextMesh.text = "";

            splitMessage = text.Split(' ');

            StartCoroutine(StartAnimation(startDelay));
      }

      private IEnumerator StartAnimation(float startDelay)
      {
          yield return new WaitForSeconds(startDelay);

          currentText = "";

          // Store first word
          previousWord = splitMessage[0]; // Save previous word with no style            
          displayedTextMesh.text = "<i><color=yellow>" + splitMessage[indexWord] + "</color></i>";

          indexWord = 1;
          while (indexWord < splitMessage.Length)
          {
              yield return new WaitForSeconds(delayBetweenWord);
              // Add to current text previous word (no style) + next one with style
              currentText += previousWord + " ";
              displayedTextMesh.text = currentText + "<i><color=yellow>" + splitMessage[indexWord] + "</color></i>";

              // Save previous word with no style and add 1
              previousWord = splitMessage[indexWord];
              indexWord += 1;

          }

          yield return new WaitForSeconds(delayBetweenWord);
          displayedTextMesh.text = textToAnimate;
          yield return new WaitForSeconds(5);
          displayedTextMesh.text = "";
      }
}
