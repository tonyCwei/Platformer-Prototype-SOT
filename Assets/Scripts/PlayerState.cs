using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float maxHealth = 100;
    public float currHealth;
    public float maxStamina = 100;
    public float currStamina;
    public float maxEnergy = 50;
    public float currEnergy;
    public float staminaRecoveryDelay = 2f;
    public int deathRewindCount;

    private Coroutine regen;

    public PlayerMovement myPlayerMovement;
    public Animator myAnimator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        currStamina = maxStamina;
        currEnergy = 0;
        deathRewindCount = 5;
        
    }

    // Update is called once per frame
    void Update()
    {
       if (!myPlayerMovement.isAlive) {
         if (regen != null) {
         StopCoroutine(regen);
         }
       }

       
    }

    public void UseStamina(int amount) {     
        currStamina -= amount; 
        if (regen != null) {
            StopCoroutine(regen);
        }
        regen = StartCoroutine(RefillStamina());

    }

    IEnumerator RefillStamina(){
    
    yield return new WaitForSeconds(staminaRecoveryDelay);
    Debug.Log("refill started");
    while (currStamina < maxStamina ) {
    currStamina +=   maxStamina / 100;
    yield return new WaitForSeconds(0.05f);
    }
    }


    public void UseHealth(int amount) {
        currHealth -= amount;
    }


      public void UseEnergy(float amount) {
        currEnergy -= amount;
    }  

    public void AddEnergy(float amount) {
        if (currEnergy + amount <= maxEnergy) {
            currEnergy += amount;
        } else {
            currEnergy = maxEnergy;
        }
    }
 

   private void OnTriggerEnter2D(Collider2D other) {  
    if (other.tag == "Mana" && !myPlayerMovement.isRewinding 
         && currEnergy < maxEnergy) {
       AddEnergy(10);
       Destroy(other.gameObject);
    }
   }



}
