using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingGauntlet : Tool
{
    public override void Action()
    {
        if (onPlayer && person.IsOnWall){
            person.CanWallJump = true;
        }
    }
}
