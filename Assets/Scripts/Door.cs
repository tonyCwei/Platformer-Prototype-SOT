using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    public float openSpeed;

    public bool isOpened = false;
    
    float startPosition;
    public float diff = 0;
   
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        startPosition = transform.position.y;
    }

    
    void Update()
    {
        DoorCheck();
    }

    public void DoorController() {
        if (isOpened) {
            DoorClose();
            //StartCoroutine(DoorClose());
        } else {
            //StartCoroutine(DoorOpen());
            DoorOpen();
        }

    }

     void DoorOpen(){
       myRigidBody.velocity = new Vector2(0, openSpeed);  
    }

     void DoorClose(){ 
       myRigidBody.velocity = new Vector2(0, -openSpeed);  
    }

    void DoorCheck(){
       float curPosition = transform.position.y;
       diff = curPosition - startPosition;

       if(diff < 0 && isOpened){
        myRigidBody.velocity = new Vector2(0,0);
        isOpened = false;
       } 
       
       if (diff > 3 && !isOpened) { 
         myRigidBody.velocity = new Vector2(0,0);
         isOpened = true;
       }

    }
   
    // IEnumerator DoorOpen(){
    //     isOpened = true;
    //     myRigidBody.velocity = new Vector2(0, openSpeed);
    //     yield return new WaitForSeconds(openTime);
    //     myRigidBody.velocity = new Vector2(0,0);
    // }
   

    // IEnumerator DoorClose(){
    //     isOpened = false;
    //     myRigidBody.velocity = new Vector2(0, -openSpeed);
    //     yield return new WaitForSeconds(openTime);
    //     myRigidBody.velocity = new Vector2(0,0);
    // }

} // End of Class
