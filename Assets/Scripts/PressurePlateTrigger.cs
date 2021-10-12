using System.Collections;
using System.Collections.Generic;
using PersonAndGhost.Utils;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;
    private IDoor door;

    private void Awake() {
        door = doorObject.GetComponent<IDoor>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag(Utility.LEFTPLAYERTAG)) {
            if(doorObject) {
                door.Open();
            }
        }
    }

}
