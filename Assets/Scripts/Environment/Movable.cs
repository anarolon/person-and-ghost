using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour 
{
    public bool isBeingPushed;
    float xPos;

    public void Start()
    {
        xPos = transform.position.x;
    }
    public void Update()
    {
        if(isBeingPushed == false)
        {
            transform.position = new Vector3(xPos, transform.position.y);
        }
        else
        {
            xPos = transform.position.x;
        }
    }
}
