using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersonAndGhost.Utils;
using PersonAndGhost.Person;

public class AIBird_Claws : MonoBehaviour 
{
    private bool isGrabbing = false;
    public bool personNearby = false;
    public GameObject person = default; 
    public bool isPersonNearby => personNearby;


// gameObject.transform.position -> returns the position of the game object the script is attached to
// gameObject -> returns a reference to the game object the script is attached to
    private void Update()
    {
        if(isGrabbing && person != null)
        {
            PickUp();
        }
    }


    public void PickUp()
    {
        isGrabbing = true;
        Debug.Log("Bird Claws: Picked up Person.");
        person.GetComponent<Rigidbody2D>().gravityScale = 0;
        person.GetComponent<PersonMovement>().IsBeingCarried = true;
        person.transform.position = transform.position - new Vector3(0,1,0);
    }

    public void Drop()
    {
        isGrabbing = false;
        Debug.Log("Bird Claws: Released Person.");
        person.GetComponent<PersonMovement>().IsBeingCarried = false; 
    }

    public void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag(Utility.LEFTPLAYERTAG)) {
            person = collision.gameObject;
            personNearby = true;
            Debug.Log("Bird Claws: Detected Person nearby.");
        }
    }

    public void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag(Utility.LEFTPLAYERTAG)) {
            person = default;
            personNearby = false;
            Debug.Log("Bird Claws: Detected Person no longer nearby.");
        }
    }

}
