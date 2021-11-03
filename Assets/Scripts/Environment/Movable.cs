using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour 
{
    public bool isBeingMoved;
    float xPos;

    public void Start()
    {
        xPos = transform.position.x;
    }
    public void Update()
    {
        if(isBeingMoved == false)
        {
            transform.position = new Vector3(xPos, transform.position.y);
        }
        else
        {
            xPos = transform.position.x;
        }
    }
}
