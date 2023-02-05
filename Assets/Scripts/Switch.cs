using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public Door Door;
    // Start is called before the first frame update
    
    private void OnTriggerEnter2D(Collider2D other) {
    
        if (other.tag == "LightAttack" || other.tag == "HeavyAttack") {
         foreach(Transform child in transform) {
            child.gameObject.SetActive(true);
         }
            Door.Open();
        }
    }




}
