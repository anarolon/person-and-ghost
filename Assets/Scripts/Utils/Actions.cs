using UnityEngine;
using System;

namespace PersonAndGhost.Utils
{
    public static class Actions
    {
        public static Action<GameObject> OnToolPickup;
        public static Action<GameObject> OnToolDrop;
        public static Action<GameObject> OnToolActionUse;

        public static Action<int> OnCollectableCollected;

        public static Action<Vector2> OnGhostMovementTriggered;
        public static Action<bool> OnPossessionTriggered;
       
        //Actions for Ghost when possessing enemies. 
        //When possessing Bird, the use of this will pick up or drop Person if they're below.
        public static Action<bool> OnStolenActionUse;

        public static Action OnPuzzleWin;
        public static Action OnPuzzleFail;
    }
}