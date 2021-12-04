using Cinemachine;
using UnityEngine;
using System.Collections;
using PersonAndGhost.Utils;
using PersonAndGhost.Person;

namespace PersonAndGhost
{
    public class CinemachineCameraCustomSettings : MonoBehaviour
    {
        [Header("Game Object to Follow")]
        [SerializeField] MultiplayerManager _playerManager = default;

        private IEnumerator Start()
        {
            if (this.gameObject.TryGetComponent(out CinemachineVirtualCamera cvc))
            {
                yield return new WaitForFixedUpdate();

                GameObject playerToFollow;

                if (_playerManager)
                {
                    PersonMovement leftPlayer = 
                        _playerManager.GetComponentInChildren<PersonMovement>();
                    playerToFollow = leftPlayer.gameObject;
                }
                
                else
                {
                    playerToFollow = GameObject.FindGameObjectWithTag
                    (
                        Utility.LEFTPLAYERTAG
                    );

                    Debug.LogWarning("Player Manager was not found.");
                }

                if (playerToFollow)
                {
                    cvc.Follow = playerToFollow.transform;
                }

                else
                {
                    Debug.LogError("GameObject with tag " + Utility.LEFTPLAYERTAG +
                        " was not found.");
                }
            }

            else
            {
                Debug.LogError(this.gameObject + " didn't had Cinemachine Virtual" +
                    " Camera component.");
            }
        }
    }
}