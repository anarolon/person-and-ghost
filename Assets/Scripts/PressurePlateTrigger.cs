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
        if (_doorObject)
        {
            if (other.gameObject.CompareTag(Utility.LEFTPLAYERTAG)) 
            {
                _door.Open();

                Utility.ActionHandler(
                    Actions.Names.OnRequestAudio, Clips.PressurePlate, this);
            }

            else if (other.gameObject.CompareTag(Utility.MONSTERTAG) 
                && other.gameObject.TryGetComponent(out AIAgent agent))
            {
                if (agent.isPossessed)
                {
                    _door.Open();

                    Utility.ActionHandler(
                        Actions.Names.OnRequestAudio, Clips.PressurePlate, this);
                }
            }
        }
    }

}
