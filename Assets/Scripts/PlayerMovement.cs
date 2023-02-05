using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 5;
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float climbSpeed = 5;
    [SerializeField] float deathKick = 1;
    
    [SerializeField] float rollSpeed = 5;
    [SerializeField] float rollDuration = 5;
    
    [SerializeField] float backStepSpeed = 5;
    [SerializeField] float backDuration = 5;
    
    [SerializeField] float deathRewindInvinDuration = 5;
    public bool isRewinding = false;
    List<Vector3> positions;
    List<float> playerVitals;
    // List<KeyValuePair<float,float>> playerVitals;
    [SerializeField] float rewindTimeLimit = 3;
    private float PrevAnimationSpeed;
    
    
    
    public GameObject lightAttack;
    public float lightAttackSpeed = 0.3f;
    public float lightAttackTime;
    public int lightAttackStamina = 25;
    
    
    private bool attackBlocked= false;
   
    public GameObject heavyAttack;
    public float heavyAttackSpeed = 0.3f;
    public float heavyAttackTime;
    public int heavyAttackStamina = 45;
    


    
    public PlayerState myPlayerState;
   

    private float gravityScaleAtStart;
    
    
    public bool isAlive = true;
    public bool gameEnded = false;
    public bool isInvin = false;
    public bool isRevivable = true;
    public bool isHit = false;
    
    
    
    public bool isClimbing = false;
    public bool isNearLadder = false;
    public bool isGrounded = false;
    public bool isAttcking = false;
    
    public bool isRolling = false;

    
    

    public bool playerHasVertialSpeed = false;
    public bool playHasHorizontalSpeed = false;
    
    
    
    public Vector2 moveInput;
    public Rigidbody2D myRigidbody;
    public Animator anima;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    CircleCollider2D myDeathCollider;
    SpriteRenderer mySpriteRenderer;

    public GameObject endMenu;
    public GameObject inGameMenu;
    public TMP_Text endText;
   

    // Start is called before the first frame update
    void Start()
    {    

        myRigidbody = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myDeathCollider = GetComponent<CircleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        positions = new List<Vector3>();
        //playerVitals = new List<KeyValuePair<float,float>>();
        playerVitals = new List<float>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        Physics2D.IgnoreLayerCollision(11,8,false);
        
    }

    // Update is called once per frame
    void Update()
    {
        isNearLadder = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        isGrounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isAttcking =    anima.GetCurrentAnimatorStateInfo(0).IsName("LightAttack") 
                     || anima.GetCurrentAnimatorStateInfo(0).IsName("HeavyAttack");
        playerHasVertialSpeed = Mathf.Abs(myRigidbody.velocity.y) > 0.01;
        playHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > 0.01;
        isRevivable = myPlayerState.deathRewindCount > 0  && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards"));

        
        
        if (!isAlive || isHit || gameEnded) {return;}
        Run();
        FlipSprite();
        JumpAnimation();
        ClimbLadder();
        
        Hurt();
        Die();
        LiveRewind();

        

        
        // Debug.Log(moveInput);   
    }

    void FixedUpdate() {
        if (!isAlive || gameEnded) {return;}


        if (isRewinding) {
           Rewind();
        } else {
            Record();
        }
   
      
        
    }
    
    // Gets Called when "Player Input" component receives key inputs under move
    void OnMove(InputValue value) {
        if (!isAlive && !isRewinding) {return;}
        moveInput = value.Get<Vector2>();
    }

    void Run() {      
        if (!isAlive || isRewinding || isRolling) {return;}
        
        if (isAttcking) {
            myRigidbody.velocity = new Vector2(moveInput.x * 0.4f, myRigidbody.velocity.y);
        } else { 
        
        myRigidbody.velocity = new Vector2(moveInput.x * runSpeed , myRigidbody.velocity.y);
        

        
        if (isGrounded) {                
        bool isMovingHorizontal = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        anima.SetBool("isRun", isMovingHorizontal);
         } else {
            anima.SetBool("isRun", false);
         }
        }
               
    }
    

    // character facing the correct direction
    void FlipSprite(){ 
        bool isMovingHorizontal = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;  
        if (isMovingHorizontal && !isRolling) {       
        transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x)*1.3f, 1.3f);
        } 
    }
 
   // Jump is different from move, when moving, key keeps being pressed. When jump, it only 
   //  needs to pressed once
    void OnJump(InputValue value) {
        if (!isAlive || isRolling|| isRewinding || isHit|| gameEnded) {return;}

        if (value.isPressed && isGrounded) {
            anima.SetTrigger("jump");
            myRigidbody.velocity = new Vector2(0f, jumpSpeed);
        } 
    }

    void JumpAnimation() {
        if (isRewinding) {return;}
        
        if(!isGrounded){
         bool isRising = myRigidbody.velocity.y > 0;
         bool isFalling = myRigidbody.velocity.y < 0;
         anima.SetBool("isFall", isFalling);
         anima.SetBool("isRise", isRising);
         } else {
            anima.SetBool("isFall", false);
          anima.SetBool("isRise", false);
         }  
     }

  //Climb Ladder
void ClimbLadder(){
  if (isRolling) {return;}

 
  if ( ((Input.GetKey("w") || Input.GetKey("s")) && isNearLadder) )
         {isClimbing = true;}  


    if (isClimbing) {
        myRigidbody.gravityScale = 0;
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveInput.y*climbSpeed);
        bool playerHasSpeed = playerHasVertialSpeed || playHasHorizontalSpeed;          
        anima.SetBool("isClimb", playerHasSpeed); 
        anima.SetBool("isClimbIdle", !playerHasSpeed);
    }
    
   if (!isNearLadder){ 
    isClimbing = false;         
    myRigidbody.gravityScale = gravityScaleAtStart;
    anima.SetBool("isClimb", false);
    anima.SetBool("isClimbIdle", false);
    } 

    
    if (isGrounded ){
        anima.SetBool("isClimbIdle", false);
        anima.SetBool("isClimb", false); 
    }
}
    

//Roll
void OnRoll(InputValue value) {
    if(!isAlive || !isGrounded || isRolling || isAttcking || isRewinding || isHit || gameEnded) {return;}

 

    if (value.isPressed) {
    if(!playHasHorizontalSpeed && anima.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
         Debug.Log("backstep");
         StartCoroutine(BackStep());
        anima.SetTrigger("backStep");
    }

    if (playHasHorizontalSpeed 
        && Mathf.Sign(transform.localScale.x) == Mathf.Sign(myRigidbody.velocity.x)){
    Debug.Log("onRoll");    
    StartCoroutine(Roll());
    anima.SetTrigger("roll");
    }
    
    }
}    

IEnumerator Roll(){
isInvin = true;
isRolling = true;
myRigidbody.velocity = new Vector2(rollSpeed * Mathf.Sign(transform.localScale.x), myRigidbody.velocity.y);
Physics2D.IgnoreLayerCollision(11,8,true);   
yield return new WaitForSeconds (rollDuration);
isInvin = false;
isRolling = false;
Physics2D.IgnoreLayerCollision(11,8,false);
}

IEnumerator BackStep(){
isInvin = true;
isRolling = true;
myRigidbody.velocity = new Vector2(backStepSpeed * -Mathf.Sign(transform.localScale.x), myRigidbody.velocity.y);   
Physics2D.IgnoreLayerCollision(11,8,true);
yield return new WaitForSeconds (backDuration);
isInvin = false;
isRolling = false;
Physics2D.IgnoreLayerCollision(11,8,false);
}





// Attack
  void OnLightAttack(InputValue value) {
     if (!isAlive || isRolling || isRewinding || attackBlocked || isHit || myPlayerState.currStamina < lightAttackStamina || gameEnded) {return;}
     if (!isGrounded && isClimbing) {
        return;
     }
     if (value.isPressed) {     
        myPlayerState.UseStamina(lightAttackStamina);      
        anima.SetTrigger("lightAttack"); // light attack animation  
        StartCoroutine(DelayLightAttack());
        StartCoroutine(EndLightAttack());
        StartCoroutine(BlockLightAttack());      
     }
  }

 IEnumerator BlockLightAttack(){
  attackBlocked = true;
   
  yield return new WaitForSeconds(lightAttackSpeed);
  attackBlocked = false;
 }

 IEnumerator DelayLightAttack(){
    isInvin = true;
    yield return new WaitForSeconds(lightAttackTime);
    lightAttack.SetActive(true); 
 }

 IEnumerator EndLightAttack(){
    yield return new WaitForSeconds(lightAttackTime + 0.1f);
    lightAttack.SetActive(false);
    isInvin = false;  
 }


// HeavyAttack

  void OnHeavyAttack(InputValue value) {
     if (!isAlive || isRolling || isRewinding || attackBlocked || isHit || myPlayerState.currStamina < heavyAttackStamina || gameEnded) {return;}
     if (!isGrounded && isClimbing) {
        return;
     }
     if (value.isPressed) {     
        myPlayerState.UseStamina(heavyAttackStamina);      
        anima.SetTrigger("heavyAttack"); // light attack animation  
        StartCoroutine(DelayHeavyAttack());
        StartCoroutine(EndHeavyAttack());
        StartCoroutine(BlockHeavyAttack());      
     }
  }

 IEnumerator BlockHeavyAttack(){
  attackBlocked = true;
   
  yield return new WaitForSeconds(heavyAttackSpeed);
  attackBlocked = false;
 }

 IEnumerator DelayHeavyAttack(){
    isInvin = true;
    yield return new WaitForSeconds(heavyAttackTime);
    heavyAttack.SetActive(true); 
 }

 IEnumerator EndHeavyAttack(){
    yield return new WaitForSeconds(heavyAttackTime + 0.1f);
    heavyAttack.SetActive(false);
    isInvin = false;  
 }



//    void OnHeavyAttack(InputValue value) {
//     if (!isAlive || isRolling || isAttcking || isRewinding) {return;}


//      if (!isGrounded && isClimbing) {
//         return;
//      }
//      if (value.isPressed) {        
//         anima.SetTrigger("heavyAttack"); // heavy attack animation
//      }   
     
//   }


  // Player Mortality

void Hurt() {
   if (isInvin || !isAlive) {return;}

    if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"))) {
    anima.SetBool("isRun", false);
    anima.SetBool("isFall", false);
    anima.SetBool("isRise", false);
    anima.SetBool("isClimb", false);
    anima.SetBool("isClimbIdle", false);
        myPlayerState.UseHealth(25);
        if (myPlayerState.currHealth <=0) {return;}
        anima.SetTrigger("hurt");
        StartCoroutine(Hit());
        StartCoroutine(HitInvin());
    }
}

IEnumerator Hit(){
isHit = true;
myRigidbody.velocity = new Vector2(0,0);
mySpriteRenderer.color = Color.red;
yield return new WaitForSeconds(0.5f);
isHit = false;
mySpriteRenderer.color = Color.white;
}

IEnumerator HitInvin(){
  Physics2D.IgnoreLayerCollision(11,8,true);
  yield return new WaitForSeconds(1f);
  Physics2D.IgnoreLayerCollision(11,8,false);
}






void Die() { 
//    if(isInvin) {return;} 

//    if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")) || myPlayerState.currHealth <= 0) {
       
//       Physics2D.IgnoreLayerCollision(11,8,true);

//       if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards"))) {
//         myPlayerState.currHealth = 0;
//         isRevivable = false;
//       }

if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards"))) {
        myPlayerState.currHealth = 0;
    }

if (myPlayerState.currHealth <= 0) {
       
    Physics2D.IgnoreLayerCollision(11,8,true);

    isAlive = false;
    moveInput = new Vector2(0,0);
    myRigidbody.velocity = new Vector2(0,deathKick);
    anima.SetBool("isRun", false);
    anima.SetBool("isFall", false);
    anima.SetBool("isRise", false);
    anima.SetBool("isClimb", false);
    anima.SetBool("isClimbIdle", false);
    anima.SetTrigger("dying");
   }

   EndGame();
}


//Rewind
void OnDeathRewind(){
  if (!isAlive && isRevivable) {
    
     myPlayerState.deathRewindCount--;
     myPlayerState.currHealth = 50;
     myPlayerState.currStamina = 100;
     Debug.Log("Death Rewind");
     anima.SetTrigger("deathRewind");
     StartCoroutine(DeathRewind());
  }
}

IEnumerator DeathRewind() {
Physics2D.IgnoreLayerCollision(11,8,true);
isInvin = true;
isRewinding = true;
yield return new WaitForSeconds (deathRewindInvinDuration);
isAlive = true;
isInvin = false;
isRewinding = false;
myRigidbody.velocity = new Vector2(0,0);
Physics2D.IgnoreLayerCollision(11,8,false);
}



// Live Rewind
void LiveRewind(){
   if (Input.GetKeyDown("r")) {
     StartRewind();
   }

   if (Input.GetKeyUp("r")) {
    StopRewind();
   } 
}

void StartRewind(){
    // if(myPlayerState.currEnergy <= 0) {return;}
    //Time.timeScale = 0.8f;
    Physics2D.IgnoreLayerCollision(11,8,true);
    isRewinding = true;
    isInvin = true;
    //myRigidbody.isKinematic = true;
    //PrevAnimationSpeed = anima.speed;
    
    

}

void StopRewind(){
    Time.timeScale = 1;
    Physics2D.IgnoreLayerCollision(11,8,false);
    isRewinding = false;
    isInvin = false;
    //myRigidbody.isKinematic = false;
    anima.SetBool("isRewind", false);
    //anima.speed = PrevAnimationSpeed;
    
}

void Rewind(){
    

    if (positions.Count > 0 && myPlayerState.currEnergy > 0 ){ //&& playerVitals.Count > 0){
        // if(transform.position == positions[positions.Count - 1]){
        //     positions.RemoveAt(positions.Count - 1);
        // } else {
    anima.SetBool("isRewind", true);
    //anima.speed = 0;
    transform.position = positions[positions.Count - 1];
    positions.RemoveAt(positions.Count - 1);
    // myPlayerState.currHealth = playerVitals[playerVitals.Count - 1];
    // playerVitals.RemoveAt(playerVitals.Count - 1);
     myPlayerState.UseEnergy(0.3f);
     //}





    // myPlayerState.currHealth = playerVitals[playerVitals.Count - 1].Key;  
    //myPlayerState.currStamina = playerVitals[playerVitals.Count - 1].Value;
    } else {
        StopRewind();
    }

}



void Record(){
    //fixed update run 50 time per sec, so 150 elements stores 3 seconds of positions.
    // Time.fixedDeltaTime gets time between each fixed update call

    if (positions.Count > Mathf.Round(rewindTimeLimit / Time.fixedDeltaTime )) {          
        positions.RemoveAt(0);   
    }
    if (positions.Count == 0) {positions.Add(transform.position);} 
    if (positions.Count > 0 && transform.position != positions[positions.Count - 1]) {
    positions.Add(transform.position);
    }
    //playerVitals.Add(new KeyValuePair<float,float> (myPlayerState.currHealth, myPlayerState.currStamina));
    
}


void EndGame(){
  if(!isAlive && !isRevivable){
    inGameMenu.SetActive(false);
    endMenu.SetActive(true);
    endText.text = "You Died";
  }

}




} // class end
