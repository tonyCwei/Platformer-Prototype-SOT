using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    public float openSpeed;
    public float openTime;

    bool isOpened;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open() {
        if (isOpened) {return;}
      StartCoroutine(DoorOpen());
    }
   
    IEnumerator DoorOpen(){
        isOpened = true;
        myRigidBody.velocity = new Vector2(0, openSpeed);
        yield return new WaitForSeconds(openTime);
        myRigidBody.velocity = new Vector2(0,0);
    }

}
