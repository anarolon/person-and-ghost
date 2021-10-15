using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour 
{
    public void Break()
    {
        // TODO: Play Break Animation
        GameObject.Destroy(gameObject);
    }
}
