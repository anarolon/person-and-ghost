using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using PersonAndGhost.Utils;
// using PersonAndGhost.Person;


public class AIActionState : AIState
{   
    //public bool actionTracker = false;
    //PersonMovement _nearbyPerson = default;

    public AIStateId GetId()
    {
        return AIStateId.Action;
    }
    public void Enter(AIAgent agent)
    {
       
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        // if(agent.CanAct)
        // {
        //     Action(agent);
        //     agent._isActing = false;
        // }
    }

    public void Action(AIAgent agent)
    {
        // if( Equals( agent.gameObject.name, "Bird" ) ) 
        // {
        //     Debug.Log("I am acting");
        //     //Person is not yet picked up by Bird, pick them up.
        //     if(!actionTracker)
        //     {

        //         actionTracker = true;
        //     }
        //     //Person is currently being picked up by Bird, drop them.
        //     else
        //     {

        //         actionTracker = false;
        //     }
        // }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag(Utility.LEFTPLAYERTAG))
    //     {
    //         _nearbyPerson = collision.gameObject.GetComponent<PersonMovement>();
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag(Utility.LEFTPLAYERTAG))
    //     {
    //         _nearbyPerson = default;
    //     }
    // }

}
