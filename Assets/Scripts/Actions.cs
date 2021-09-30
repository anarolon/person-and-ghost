using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Actions
{
    public static Action<GameObject> OnToolPickup;
    public static Action<GameObject> OnToolDrop;
    public static Action<GameObject> OnToolActionUse;

    public static Action<int> OnCollectableCollected;

    public static Action OnPuzzleWin;
    public static Action OnPuzzleFail;

}