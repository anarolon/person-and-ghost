using UnityEngine;
using UnityEngine.InputSystem;
using PersonAndGhost.Utils;

namespace PersonAndGhost.Ghost
{
    [RequireComponent(typeof(SpriteRenderer), typeof(CapsuleCollider2D))]
    public class GhostPossession : MonoBehaviour
    {
        [SerializeField] private GhostConfig _config = default;
        private SpriteRenderer _renderer = default;
        private AIAgent _nearbyMonster = default;
        private AIAgent _monster = default;
        private Vector2 _movement = Vector2.zero;

        public bool IsPossessing { get; private set; } = false;
        public bool IsNearAMonster => _nearbyMonster;

        private void Awake()
        {
            if (!_config)
            {
                _config = ScriptableObject.CreateInstance<GhostConfig>();
            }

            transform.localScale = _config.scale;

            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sortingOrder = _config.sortingOrder;
            _renderer.sprite = _config.sprite;
            _renderer.color = _config.color;
            if(!_renderer.sprite)
            {
                _renderer.sprite = Resources.Load<Sprite>(Utility.CAPSULESPRITE);
            }

            CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
            collider.isTrigger = _config.isTrigger;
            collider.size = _config.size;
        }

        private void OnEnable()
        {
            Actions.OnGhostMovementTriggered += UpdateMovement;
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
            if (collision.gameObject.CompareTag(Utility.MONSTERTAG) && !IsPossessing)
            {
                _nearbyMonster = collision.gameObject.GetComponent<AIAgent>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Utility.MONSTERTAG) && !IsPossessing)
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

        //ON STOLEN ACTION
        public void OnStolenAction(InputAction.CallbackContext context)
        {
            if (IsPossessing)
            {
                _monster._isActing = context.action.triggered;
            }
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

            try
            {
                Actions.OnPossessionTriggered(IsPossessing);
            }

            catch (System.NullReferenceException)
            {
                // Do nothing
            }

            // Possess the nearby creature.
            if (IsPossessing)
            {
                _monster = _nearbyMonster;
                _monster.stateMachine.ChangeState(AIStateId.Possessed);
                _renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
            
            // Release the possessed creature.
            else
            {
                _monster.stateMachine.ChangeState(_monster.initialState);
                _renderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                _monster = null;
            }
        }

        private void OnDisable()
        {
            Actions.OnGhostMovementTriggered -= UpdateMovement;
        }
    }
}