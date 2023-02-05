using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndText : MonoBehaviour
{
    TMP_Text mytext;
    // Start is called before the first frame update
    void Start()
    {
        mytext = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string message){
       mytext.text = message;
    }
}
