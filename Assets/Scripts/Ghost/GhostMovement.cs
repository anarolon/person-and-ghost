using UnityEngine.InputSystem;
using UnityEngine;
using PersonAndGhost.Utils;

namespace PersonAndGhost.Ghost
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GhostMovement : MonoBehaviour
    {
        [SerializeField] private GhostConfig _config = default;
        private float _movementAcceleration = 50f;
        private float _maxMoveSpeed = 12f;
        private float _linearDrag = 10f;
        private Rigidbody2D _rigidbody = default;
        private Vector2 _movementInput = Vector2.zero;
        private bool _isPossessing = false;

        private int _direction = 1;
        private SpriteRenderer _ghostSprite;

        private void Awake()
        {
            if (!_config)
            {
                _config = ScriptableObject.CreateInstance<GhostConfig>();
            }

            _movementAcceleration = _config.movementAcceleration;
            _maxMoveSpeed = _config.maxMoveSpeed;
            _linearDrag = _config.linearDrag;

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = _config.gravityScale;
            _rigidbody.collisionDetectionMode = _config.collisionDetectionMode;
            _rigidbody.constraints = _config.constraints;

            _ghostSprite = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            Actions.OnPossessionTriggered += UpdatePossession;
        }

        private void FixedUpdate()
        {
            if (!_isPossessing)
            {
                MoveGhost();
                if(_ghostSprite) _ghostSprite.flipX = _direction > 0 ? false : true;
                   
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();

            try
            {
                Actions.OnGhostMovementTriggered(_movementInput);
            }

            catch (System.NullReferenceException)
            {
                // Do nothing
            }
        }

        private void UpdatePossession(bool isPossessing)
        {
            _isPossessing = isPossessing;
        }

        private void MoveGhost()
        {
            _rigidbody.AddForce(_movementInput * _movementAcceleration);
            if (_movementInput.x > 0) _direction = 1;
            else if (_movementInput.x < 0) _direction = -1;

            var velocity = _rigidbody.velocity;

            if (Mathf.Abs(velocity.x) > _maxMoveSpeed 
                || Mathf.Abs(velocity.y) > _maxMoveSpeed)
            {
                float x = Mathf.Sign(velocity.x) * _maxMoveSpeed;
                float y = Mathf.Sign(velocity.y) * _maxMoveSpeed;

                _rigidbody.velocity = new Vector2(x, y);
            }

            _rigidbody.drag = _linearDrag;
        }

        private void OnDisable()
        {
            Actions.OnPossessionTriggered -= UpdatePossession;
        }
    }
}