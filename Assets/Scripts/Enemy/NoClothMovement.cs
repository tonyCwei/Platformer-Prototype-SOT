using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoClothMovement : MonoBehaviour
{
    
    [SerializeField] float moveSpeed = 1f;


    [SerializeField] int maxHealth = 100;
    public int currHealth;
    public GameObject healthBar;
    public Slider slider;
    
    [SerializeField] float hitDuration = 1;
    [SerializeField] float hitKick;
    
    
    public GameObject player;
    public PlayerMovement myPlayerMovement;
    public GameObject ManaOrb;

    
    
    float defaultAnimationSpeed;

    
    
    bool isAlive = true;
    bool isHit = false;
    public bool isRage = false;

    
    
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed *= Mathf.Sign(transform.localScale.x);
        currHealth = maxHealth; 
        defaultAnimationSpeed = myAnimator.speed;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {return;} 
        Move();
        Followplayer();
        Die();
        FlipSprite();
        
       slider.value = currHealth;
       

        
    }

    void Move(){
        if(isHit) {return;}
        myRigidbody.velocity = new Vector2(moveSpeed,myRigidbody.velocity.y);
        
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Ground") {
        moveSpeed = -moveSpeed;
        
        }
    }


    void FlipSprite() {

        transform.localScale = new Vector2(Mathf.Sign(moveSpeed),1f);
        if (moveSpeed > 0) {
        slider.direction = Slider.Direction.LeftToRight;
        } else {
          slider.direction = Slider.Direction.RightToLeft;
        }
    }

  



    void Die(){
        if (currHealth <=0) {
            isAlive = false;
            healthBar.SetActive(false);
            mySpriteRenderer.color = Color.white;
            myAnimator.SetTrigger("dying");
             myRigidbody.velocity = new Vector2(0,0);
             myRigidbody.isKinematic = true;
             Destroy(GetComponent<CapsuleCollider2D>());
             Destroy(GetComponent<BoxCollider2D>());
            StartCoroutine(SpawnMana());
            }

         if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))  {
          myAnimator.SetTrigger("dying");
          Destroy(gameObject,0.9f);
         }  
        
    }

    IEnumerator SpawnMana(){
      yield return new WaitForSeconds(0.8f);
      Instantiate(ManaOrb,transform.position, transform.rotation);
      Destroy(gameObject);
      
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (isHit || !isAlive) {return;}
        
        

        if (other.tag == "LightAttack" || other.tag == "HeavyAttack") {        
            isRage = true;
            healthBar.SetActive(true);
             if (moveSpeed>0) {
               moveSpeed += 0.5f;
            } else {
                moveSpeed -= 0.5f;
            }



            // if (other.tag == "LightAttack") {
            // currHealth -= 20;
            // if(currHealth <=0) {return;}
            // StartCoroutine(LightHit());
            // } else {
            //   currHealth -= 40;
            // if(currHealth <=0) {return;}
            // StartCoroutine(HeavyHit());
            // }

          switch(other.tag) {
            case "LightAttack":
            currHealth -= 20;
            if(currHealth <=0) {return;}
            StartCoroutine(LightHit());
            break;

            case "HeavyAttack":
            currHealth -= 40;
            if(currHealth <=0) {return;}
            StartCoroutine(HeavyHit());
            break;
          }



         

           

        }
    }


  IEnumerator LightHit(){
   float kickDirection = -Mathf.Sign(player.transform.position.x - transform.position.x);
   isHit = true;
   myRigidbody.velocity = new Vector2(hitKick * kickDirection, 5f);
   mySpriteRenderer.color = Color.red;
   yield return new WaitForSeconds(hitDuration);
   isHit = false;
   mySpriteRenderer.color = Color.white;
  }

   IEnumerator HeavyHit(){
   float kickDirection = -Mathf.Sign(player.transform.position.x - transform.position.x);
   isHit = true;
   myRigidbody.velocity = new Vector2(hitKick * kickDirection*1.5f, 1.5f * 5f);
   mySpriteRenderer.color = Color.red;
   yield return new WaitForSeconds(hitDuration);
   isHit = false;
   mySpriteRenderer.color = Color.white;
  }






  void Followplayer(){
    if (!isRage || !myPlayerMovement.isAlive || myPlayerMovement.isHit ) {return;}
    float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
    if ( direction != Mathf.Sign(moveSpeed)){
            moveSpeed *= -1;
    }


  }


  // 




} // class end