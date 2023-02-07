using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaOrb : MonoBehaviour
{

    Rigidbody2D myRigidbody;
    BoxCollider2D myBoxCollider;
    public PlayerMovement myPlayerMovement;
    public float moveSpeed;

    Vector3 directionToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
       // StartCoroutine(Spawn());
        myPlayerMovement = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Followplayer();
    }

    void Followplayer(){

        if(!myPlayerMovement.isAlive) {return;}


        directionToPlayer = (myPlayerMovement.transform.position - transform.position).normalized;
        myRigidbody.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed;
    
    }

    // IEnumerator Spawn(){
    //     myRigidbody.velocity = new Vector2(Random.Range(-0.5f,0.5f),Random.Range(0.1f,0.5f));
    //     yield return new WaitForSeconds(5f);
    //     myRigidbody.velocity = new Vector2(0,0);

    // }
 
}
