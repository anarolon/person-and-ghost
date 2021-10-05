using UnityEngine.InputSystem;
using UnityEngine;
using PersonAndGhost.Utils;

namespace PersonAndGhost.Ghost
{
    public class GhostMovement : MonoBehaviour
    {
        [Header("Movement Fields")]
        [SerializeField] private float _movementAcceleration = 50f;
        [SerializeField] private float _maxMoveSpeed = 12f;
        [SerializeField] private float _linearDrag = 10f;
        private Rigidbody2D _rigidbody2D = default;

        private bool _isPossessing = false;

        public Vector2 MovementInput { get; private set; }

        private void Start()
        {
            if (TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                _rigidbody2D = rigidbody2D;
            }

            else
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
                _rigidbody2D.gravityScale = 0;

                Debug.LogWarning("Rigidbody 2D component was not attached.");
            }
        }

        private void FixedUpdate()
        {
            if (!_isPossessing)
            {
                MoveGhost();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();

            Actions.OnGhostMovementTriggered(MovementInput);
        }

        private void OnEnable()
        {
            Actions.OnPossessionTriggered += UpdatePossession;
        }

        private void OnDisable()
        {
            Actions.OnPossessionTriggered -= UpdatePossession;
        }

        private void UpdatePossession(bool isPossessing)
        {
            _isPossessing = isPossessing;
        }

        private void MoveGhost()
        {
            _rigidbody2D.AddForce(MovementInput * _movementAcceleration);

            var velocity = _rigidbody2D.velocity;
            if (Mathf.Abs(velocity.x) > _maxMoveSpeed || Mathf.Abs(velocity.y) > _maxMoveSpeed)
            {
                var x = Mathf.Sign(velocity.x) * _maxMoveSpeed;
                var y = Mathf.Sign(velocity.y) * _maxMoveSpeed;
                _rigidbody2D.velocity = new Vector2(x, y);
            }

            _rigidbody2D.drag = _linearDrag;
        }
    }
}