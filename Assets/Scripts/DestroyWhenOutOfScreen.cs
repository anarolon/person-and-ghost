using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PersonAndGhost
{
    public class DestroyWhenOutOfScreen : MonoBehaviour
    {
        private Camera _mainCamera = default;
        private GhostMovement _ghostIfMonster;
        private GameObjectType _gameObjectType = default;
        private AIAgent _agentIfMonster = default;

        private enum GameObjectType
        {
            Monster,
            Person
        }

        private void Start()
        {
            _mainCamera = Camera.main;

            if (TryGetComponent(out AIAgent agent))
            {
                _agentIfMonster = agent;
                _ghostIfMonster = GameObject.FindGameObjectWithTag("Ghost")
                    .GetComponent<GhostMovement>();
                _gameObjectType = GameObjectType.Monster;
            }

            else if (TryGetComponent(out PlayerController _))
            {
                _gameObjectType = GameObjectType.Person;
            }
        }

        private void FixedUpdate()
        {
            if (!PersonAndGhost.Utils.Utility.IsVisibleToCamera(_mainCamera, transform.position))
            {
                if (_gameObjectType == GameObjectType.Monster)
                {
                    UnpossessMonster();
                    Destroy(this.gameObject);
                }

                else if (_gameObjectType == GameObjectType.Person)
                {
                    Actions.OnPuzzleFail();
                    Destroy(this.gameObject.transform.parent.gameObject);
                }
            }
        }

        private void UnpossessMonster()
        {
            if (_agentIfMonster.stateMachine.currentState == AIStateId.Possessed)
            {
                _ghostIfMonster.OnPossession(new InputAction.CallbackContext());
                _ghostIfMonster.gameObject.transform.position =
                    _ghostIfMonster.Anchor.position;
            }
        }
    }
}