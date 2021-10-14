using PersonAndGhost.Utils;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _doorObject;
    private IDoor _door;

    private void Awake() 
    {
        _door = _doorObject.GetComponent<IDoor>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag(Utility.LEFTPLAYERTAG)
            || other.gameObject.CompareTag(Utility.MONSTERTAG)) 
        {
            if (_doorObject) 
            {
                _door.Open();
            }
        }
    }

}
