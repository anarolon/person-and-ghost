using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingGauntlet : MonoBehaviour, Tool
{

    [SerializeField] public bool onPlayer = false;
    PlayerController person;

    public void Action()
    {
        if (onPlayer && person.IsOnWall){
            person.CanWallJump = true;
        }
        return;
    }

    public void GetDropped()
    {
        onPlayer = false;
    }

    public void GetPickedUp(PlayerController player)
    {
        onPlayer = true;
        person = player;
    }

    void Start()
    {
        onPlayer = false;
    }

    void Update()
    {
        if (onPlayer)
        {
            transform.position = person.gameObject.transform.position;
        }
    }
}
