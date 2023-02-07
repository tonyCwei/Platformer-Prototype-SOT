using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool isLit;
    public Ladder myLadder;
    public Door myDoor;
    // Start is called before the first frame update
    
    private void OnTriggerEnter2D(Collider2D other) {
       
        if (other.tag == "LightAttack" || other.tag == "HeavyAttack") {
          if (!isLit && !myDoor.isOpened) {
                isLit = true;
                 foreach(Transform child in transform) {
                 child.gameObject.SetActive(true);
                }
                 myDoor.DoorController();
                 myLadder.LadderController();
          } else if (isLit && myDoor.isOpened) {
                 isLit = false;
                 foreach(Transform child in transform) {
                 child.gameObject.SetActive(false);
                }
                myDoor.DoorController();
                myLadder.LadderController();
          }
        }
    }




}
