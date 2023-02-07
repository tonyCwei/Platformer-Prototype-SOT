using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
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
        LadderCheck();
    }

    public void LadderController() {
        if (isOpened) {
            LadderClose();
            //StartCoroutine(DoorClose());
        } else {
            //StartCoroutine(DoorOpen());
            LadderOpen();
        }

    }

     void LadderOpen(){
       myRigidBody.velocity = new Vector2(0, -openSpeed);  
    }

     void LadderClose(){ 
       myRigidBody.velocity = new Vector2(0, openSpeed);  
    }

    void LadderCheck(){
       float curPosition = transform.position.y;
       diff = curPosition - startPosition;

       if(diff > 0 && isOpened){
        myRigidBody.velocity = new Vector2(0,0);
        isOpened = false;
       } 
       
       if (diff < -3 && !isOpened) { 
         myRigidBody.velocity = new Vector2(0,0);
         isOpened = true;
       }

    }
}
