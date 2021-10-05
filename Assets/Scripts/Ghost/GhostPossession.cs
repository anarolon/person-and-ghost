using UnityEngine;
using UnityEngine.InputSystem;
using PersonAndGhost.Utils;

namespace PersonAndGhost.Ghost
{
    public class GhostPossession : MonoBehaviour
    {
        [Header("Possession Fields")]
        private AIAgent _nearbyMonster = default;
        private AIAgent _monster = default;

        private SpriteRenderer _renderer = default;
        private Vector2 _movement = Vector2.zero;

        public bool IsPossessing { get; private set; } = false;
        public bool IsNearAMonster => _nearbyMonster;

        private void Start()
        {
            if (TryGetComponent(out SpriteRenderer renderer))
            {
                _renderer = renderer;
            }

            else
            {
                _renderer = gameObject.AddComponent<SpriteRenderer>();

                Debug.LogWarning("Sprite Renderer component was not attached.");
            }
        }

        private void FixedUpdate()
        {
            if (IsPossessing && _monster)
            {
                MoveMonster();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Utility.MONSTERTAG))
            {
                _nearbyMonster = collision.gameObject.GetComponent<AIAgent>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Utility.MONSTERTAG))
            {
                _nearbyMonster = null;
            }
        }

        public void OnPossession(InputAction.CallbackContext context)
        {
            if (context.action.triggered && IsNearAMonster)
            {
                ChangePossession();
            }
        }

        public void OnMonsterJump(InputAction.CallbackContext context)
        {
            if (IsPossessing)
            {
                _monster._isJumping = context.action.triggered;
            }
        }

        private void OnEnable()
        {
            Actions.OnGhostMovementTriggered += UpdateMovement;
        }

        private void OnDisable()
        {
            Actions.OnGhostMovementTriggered -= UpdateMovement;
        }

        private void UpdateMovement(Vector2 movement)
        {
            _movement = movement;
        }

        private void MoveMonster()
        {
            _monster.movementBehaviour(_movement);
            gameObject.transform.position = _monster.transform.position;
        }

        public void ChangePossession()
        {
            IsPossessing = !IsPossessing;

            Actions.OnPossessionTriggered(IsPossessing);

            if (IsPossessing)
            {
                _monster = _nearbyMonster;
                _monster.stateMachine.ChangeState(AIStateId.Possessed);
                _renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }

            else
            {
                _monster.stateMachine.ChangeState(_monster.initialState);
                _renderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                _monster = null;
            }
        }
    }
}