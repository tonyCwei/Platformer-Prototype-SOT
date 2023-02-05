using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SavePoint : MonoBehaviour
{
    
    [SerializeField] GameObject endGameMenu;

    [SerializeField] GameObject inGameMenu;

    [SerializeField] PlayerMovement PlayerMovement;

    [SerializeField] TMP_Text endText;

    

    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {        
         if (other.tag == "Player") {
        

         foreach(Transform child in transform) {
            child.gameObject.SetActive(true);
         }
         
         
         
         
         PlayerMovement.myRigidbody.velocity = new Vector2(0,0);
         PlayerMovement.anima.SetBool("isEnd", true);
         PlayerMovement.gameEnded = true;

         inGameMenu.SetActive(false);
         endGameMenu.SetActive(true);
         endText.text = "Awesome!";
         }
    }



}// class end
