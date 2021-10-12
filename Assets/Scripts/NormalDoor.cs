using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDoor : MonoBehaviour, IDoor
{
    public void Close()
    {
        // TODO: Play Close Normal animation
    }

    public void Open()
    {
        GameObject.Destroy(gameObject);
        // TODO: Play Open Normal animation
    }
}
