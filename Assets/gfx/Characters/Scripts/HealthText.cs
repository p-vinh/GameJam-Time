using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public float timeToLive = 1f;
    public float floatSpeed = 1;
    public Vector3 floatDirection = new Vector3(0, 1);
    public TextMeshProUGUI textMesh;
    RectTransform rTransform;
    float timeElapsed = 0.0f;
    Color startingColor;
    
    void Start()
    {
        rTransform = GetComponent<RectTransform>();
        startingColor = textMesh.color;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        rTransform.position += floatDirection * floatSpeed * Time.deltaTime;
        textMesh.color = new Color(startingColor.r, startingColor.g, startingColor.b, 1 - (timeElapsed / timeToLive));

        if (timeElapsed > timeToLive) {
            Destroy(gameObject);
        }
    }
}
