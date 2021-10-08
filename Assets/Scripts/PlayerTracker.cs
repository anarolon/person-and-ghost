using UnityEngine;
using System.Collections;
using PersonAndGhost.Utils;
using PersonAndGhost.Ghost;
using PersonAndGhost.Person;

namespace PersonAndGhost
{
    public class PlayerTracker : MonoBehaviour
    {
        [Header("Game Objects to Track")]
        [SerializeField] GameObject _playerManager = default;
        private Camera _camera = default;
        private Transform _leftPlayer = default;
        private Transform _rightPlayer = default;

        private IEnumerator Start()
        {
            if (TryGetComponent(out Camera camera))
            {
                _camera = camera;
            }

            else
            {
                gameObject.AddComponent<Camera>();

                tag = Utility.MAINCAMERATAG;

                Debug.LogWarning("Camera component was not attached.");
            }

            if (!_playerManager)
            {
                _playerManager = FindObjectOfType<MultiplayerManager>().gameObject;

                Debug.LogWarning("Player Manager was not added in the inspector.");
            }

            yield return new WaitForFixedUpdate();

            PersonMovement person =
                _playerManager.GetComponentInChildren<PersonMovement>();
            GhostMovement ghost =
                _playerManager.GetComponentInChildren<GhostMovement>();

            _leftPlayer = person.gameObject.transform;
            _rightPlayer = ghost.gameObject.transform;

            StartCoroutine(TrackPlayersTransform());
        }
         
        private IEnumerator TrackPlayersTransform()
        {
            while (_playerManager)
            {

                if (!Utility.IsVisibleToCamera(_camera, _leftPlayer.position))
                {
                    Destroy(_playerManager);

                    Actions.OnPuzzleFail();
                }

                else if (!Utility.IsVisibleToCamera(_camera, _rightPlayer.position))
                {
                   // _rightPlayer.position = _leftPlayer.position;
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}