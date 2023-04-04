using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestReset : MonoBehaviour
{
    public GameObject open;
    public GameObject close;
    public SignText text;
    private void OnEnable() {        
        StartCoroutine(Reset());
    }

    private IEnumerator Reset() {
        yield return new WaitForSeconds(120);
        close.SetActive(true);
        open.SetActive(false);
    }
}
