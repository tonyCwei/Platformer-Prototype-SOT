using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaOrb : MonoBehaviour
{

    Rigidbody2D myRigidbody;
    BoxCollider2D myBoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        

    }

    IEnumerator Spawn(){
        myRigidbody.velocity = new Vector2(Random.Range(-0.5f,0.5f),Random.Range(0.1f,0.5f));
        yield return new WaitForSeconds(5f);
        myRigidbody.velocity = new Vector2(0,0);

    }
 
}
