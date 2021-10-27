using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersonAndGhost.Utils;
using PersonAndGhost.Person;

public class AIBird_Claws : MonoBehaviour 
{
    private bool isCarrying = false;
    public bool personNearby = false;
    public bool isPersonNearby => personNearby;
    public GameObject person = default;
    public AIBird bird = default;

    private void Start()
    {
        bird = gameObject.GetComponentInParent<AIBird>();
    }

    private void Update()
    {
        // Continue carrying Person after Ghost picks them up
        // as long as conditions are met.
        if(isCarrying && bird.isPossessed && personNearby && person != null)
        {
            PickUp();
        }
        // Forces drop if Ghost releases Bird or if Person is 
        // forced away from claws.
        else if((!bird.isPossessed || !personNearby) && person != null)
        {
            Drop();
            bird.isCarrying = false;
        }
    }

    public void PickUp()
    {
        //Debug.Log("Bird Claws: Picked up Person.");
        isCarrying = true;
        person.GetComponent<Rigidbody2D>().gravityScale = 0;
        person.GetComponent<PersonMovement>().IsBeingCarried = true;
        person.transform.position = transform.position - new Vector3(0, person.GetComponent<SpriteRenderer>().bounds.size.y/2, 0);
    }

    public void Drop()
    {
        //Debug.Log("Bird Claws: Released Person.");
        isCarrying = false;
        person.GetComponent<Rigidbody2D>().gravityScale = 1;
        person.GetComponent<PersonMovement>().IsBeingCarried = false;
        person = null; 
    }

    public void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag(Utility.LEFTPLAYERTAG)) 
        {   
            //Debug.Log("Bird Claws: Detected Person nearby.");
            person = collision.gameObject;
            personNearby = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag(Utility.LEFTPLAYERTAG)) 
        {
            //Debug.Log("Bird Claws: Detected Person no longer nearby.");
            personNearby = false;
        }
    }
}
