using UnityEngine;
using System;

namespace PersonAndGhost.Utils
{
    public static class Actions
    {
        public static Action<GameObject, GameObject> OnToolPickup;
        public static Action<GameObject, GameObject> OnToolDrop;
        public static Action<GameObject> OnToolActionUse;

        public static Action<int> OnCollectableCollected;

        public static Action<Vector2> OnGhostMovementTriggered;
        public static Action<bool, AIAgent> OnPossessionTriggered;

        public static Action<bool> OnRoomStateChange;
        public static Action<bool> OnFloorStateChange;

        public static Action OnGamePause;
        public static Action OnGameUnPause;

        public static Action<string> OnPersonRequestAudio;
    }
}