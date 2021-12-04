using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchDoor : MonoBehaviour, IDoor
{
    public void Close()
    {
        // TODO: Play Close Hatch animation
    }

    public void Open()
    {
        GameObject.Destroy(gameObject);
        // TODO: Play Open Hatch animation
    }
}
