using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITool
{
    public void GetPickedUp(PlayerController player);
    public void GetDropped();
    public void Action();
}
