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

        public static Action<bool> OnRoomStateChange;
    }
}