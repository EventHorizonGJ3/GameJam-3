using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAutoAnimation : MonoBehaviour
{
    
    private TextMeshProUGUI textMeshPro;

    
    [SerializeField] float minFontSize = 24f;
    [SerializeField] float maxFontSize = 48f;
    [SerializeField] float speed = 2f;

    
    

    void Start()
    {
        
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        
        float newFontSize = Mathf.Lerp(minFontSize, maxFontSize, Mathf.PingPong(Time.time * speed, 1));
        textMeshPro.fontSize = newFontSize;
    }
}
