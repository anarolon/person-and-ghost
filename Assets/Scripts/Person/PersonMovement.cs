using UnityEngine;
using UnityEngine.InputSystem;
using PersonAndGhost.Person.States;
using PersonAndGhost.Utils;
using System.Collections;

namespace PersonAndGhost.Person
{
    public class PersonMovement : MonoBehaviour
    {
        [Header("State Machine Fields")]
        public IdleState idle;
        public MovementState movement;
        public JumpingState jumping;
        public FallingState falling;
        public MeditatingState meditate;
        public ClingState cling;
        public CarriedState carried;
        public bool isDead = false;
        public bool isWinner = false;
        public GrappleAimState grappleAim;

        [Tooltip("Currently only being used to visualize current player state")]
        [SerializeField] private string _currentState = default;

        private StateMachine _movementSM;

        [Header("Cached Components and Fields")]
        [SerializeField] private PersonConfig _config = default;

        private Rigidbody2D _playerRB = default;

        [Header("Layer Masks")]
        [SerializeField] private LayerMask _groundLayer = default;
        [SerializeField] private LayerMask _wallLayer = default;

        [Header("Movement Fields")]
        private float _horizontalVelocity = default;
        private bool _changingDirection => (
            _playerRB.velocity.x > 0f && _horizontalVelocity < 0f)
            || (_playerRB.velocity.x < 0f && _horizontalVelocity > 0f);
        private bool _canJump = false;

        [Header("Collision Variables")]
        [SerializeField] private bool isOnGround = false;
        [SerializeField] private bool isTouchingWall = false;

        [Header("Input System Variables")]
        private Vector2 _movementInput = Vector2.zero;
        private bool _jumped = false;

        [Header("Minigame Fields")]
        [SerializeField] private GameObject _turret = default;
        [SerializeField] private GameObject _ghostlyInvasion = default;

        [Header("Grappling Fields")]
        private Vector2 _grapplePoint;
        private Vector2 _grappleDistanceVector;
        private bool _canGrapple = false;

        [Header("Audio Request Fields")]
        [SerializeField] private float _grappleHookShootCooldown = 0.1f;
        private bool _hasEnteredMeditation = false;
        private bool _hasEnteredCling = false;
        private bool _hasEnteredJump = false;
        private bool _hasEnteredDeath = false;
        private bool _hasEnteredGrappleShot = false;

        // PROPERTIES
        public GameObject GhostlyInvasion { get => _ghostlyInvasion; }
        public StateMachine MovementSM { get => _movementSM; }
        public bool CanJump { get => _canJump; set => _canJump = value; }
        public bool IsOnGround { get => isOnGround; }
        public bool IsTouchingWall { get => isTouchingWall; }
        public bool IsMeditating { get; set; }
        public bool IsBeingCarried { get; set; }
        public Vector2 MovementInput { get => _movementInput; }
        public bool Jumped { get => _jumped; }
        public Vector2 GrapplePoint { get => _grapplePoint; set => _grapplePoint = value; }
        public bool CanGrapple { get => _canGrapple; set => _canGrapple = value; }
        public string CurrentState { get => _currentState; set => _currentState = value; }

        void Awake()
        {
            _playerRB = GetComponent<Rigidbody2D>();
            _groundLayer = LayerMask.GetMask("Ground");
            _wallLayer = LayerMask.GetMask("Wall");
            _config ??= ScriptableObject.CreateInstance<PersonConfig>();
            if (_turret) _turret.SetActive(false);
        }

        private void Start()
        {
            _movementSM = new StateMachine();

            // TODO: Make this more flexible
            idle = new IdleState(this, _movementSM);
            movement = new MovementState(this, _movementSM);
            jumping = new JumpingState(this, _movementSM);
            falling = new FallingState(this, _movementSM);
            meditate = new MeditatingState(this, _movementSM);
            cling = new ClingState(this, _movementSM);
            carried = new CarriedState(this, _movementSM);
            grappleAim = new GrappleAimState(this, _movementSM);

            _movementSM.Initialize(idle);
        }

        private void Update()
        {
            _movementSM.CurrentState.HandleInput();

            _movementSM.CurrentState.LogicUpdate();
            _currentState = _movementSM.CurrentState.ToString();
        }

        private void FixedUpdate()
        {
            _horizontalVelocity = 0;

            _movementSM.CurrentState.PhysicsUpdate();

            UpdateAudioRequestFields();

            RequestAudio();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _jumped = context.action.triggered;
        }

        public void OnMeditation(InputAction.CallbackContext context)
        {
            bool triggered = context.action.triggered;

            //Not handle this behavior in here but in meditating state script.
            if (triggered)
            {
                IsMeditating = !IsMeditating;

                // TODO: Change this to a public method maybe to call it within meditation state
                _turret.SetActive(IsMeditating);
            }
        }

        public bool IsWalking()
        {
            return Mathf.Abs(_playerRB.velocity.x) > 0;
        }

        public void ResetVelocity()
        {
            if (_playerRB)
            {
                _playerRB.velocity = Vector2.zero;
                _playerRB.angularVelocity = 0;
                _playerRB.gravityScale = 1;
            }
            
        }

        public void Move()
        {
            CheckCollision();
            MovePlayer(_movementInput);

            // TODO: Do we need this ground check here?
            if (isOnGround)
            {
                ApplyLinearDrag();
            }
        }

        public void AirMove()
        {
            CheckCollision();
            MovePlayer(_movementInput);
            ApplyAirLinearDrag();
            ApplyFallMultiplier();
        }

        public void Jump()
        {
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, 0f);
            _playerRB.AddForce(Vector2.up * _config.jumpForce, ForceMode2D.Impulse);
        }

        public void Cling()
        {
            _playerRB.gravityScale = 0;
            _playerRB.velocity = Vector2.zero;
        }

        public void UnCling()
        {
            _playerRB.gravityScale = 1;
        }

        public void SetGrapplePoint(Vector2 direction)
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction,
                _config.grappleDistance, _groundLayer | _wallLayer);
            if (hit)
            {
                _canGrapple = true;
                _grapplePoint = hit.point;
                _grappleDistanceVector = (_grapplePoint -
                    (Vector2)transform.position).normalized;

                IEnumerator coroutine = AudioRequestWithCooldown(
                    _grappleHookShootCooldown, Clips.HookShoot);

                if (!_hasEnteredGrappleShot)
                {
                    _hasEnteredGrappleShot = true;

                    StartCoroutine(coroutine);
                }

                else
                {
                    StopCoroutine(coroutine);
                }
            }

            else
            {
                _canGrapple = false;
                
            }
        }

        public void Grapple()
        {
            float grappleSpeed = Mathf.Clamp(
                Vector2.Distance(transform.position, _grapplePoint),
                _config.grappleSpeedMin, _config.grappleSpeedMax);

            ResetVelocity();
            _playerRB.gravityScale = 0;
            _playerRB.AddForce(grappleSpeed * _config.grappleMultiplier
                * _grappleDistanceVector);
        }

        public bool DidReachGrapplePoint()
        {
            return Vector2.Distance(transform.position, _grapplePoint)
                < _config.reachedGrapplepointDistance;
            
        }


        private void MovePlayer(Vector2 movementInput)
        {
            _horizontalVelocity = movementInput.x;

            _playerRB.AddForce(new Vector2(_horizontalVelocity, 0f) 
                * _config.movementAcceleration);

            if (Mathf.Abs(_playerRB.velocity.x) > _config.maxMoveSpeed)
            {
                _playerRB.velocity = new Vector2(Mathf.Sign(_playerRB.velocity.x)
                    * _config.maxMoveSpeed, _playerRB.velocity.y);
            }

            if(movementInput == Vector2.left)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else if (movementInput == Vector2.right) {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        private void ApplyLinearDrag()
        {
            // TODO: Magic Number: 0.4f; make into a variable
            if (Mathf.Abs(_horizontalVelocity) < 0.4f || _changingDirection)
            {
                _playerRB.drag = _config.linearDrag;
            }
            else
            {
                _playerRB.drag = 0f;
            }
        }

        private void ApplyAirLinearDrag()
        {
            _playerRB.drag = _config.airLinearDrag;
        }

        private void ApplyFallMultiplier()
        {
            if (_playerRB.velocity.y < 0)
            {
                _playerRB.gravityScale = _config.fallMultiplier;
            }
            else if (_playerRB.velocity.y > 0 && !_jumped)
            {
                _playerRB.gravityScale = _config.lowJumpFallMultiplier;
            }
            else
            {
                _playerRB.gravityScale = 1f;
            }
        }

        private void CheckCollision()
        {
            isOnGround = 
                Physics2D.Raycast(transform.position + _config.groundRaycastOffset,
                    Vector2.down, _config.groundRaycastLength, _groundLayer) ||
                Physics2D.Raycast(transform.position - _config.groundRaycastOffset,
                    Vector2.down, _config.groundRaycastLength, _groundLayer
            );

            isTouchingWall = Physics2D.Raycast(transform.position,
                Vector2.right, _config.wallRaycastLength, _wallLayer)
                || Physics2D.Raycast(transform.position,
                Vector2.left, _config.wallRaycastLength, _wallLayer);
        }

        private void UpdateAudioRequestFields()
        {
            if (_playerRB.gravityScale == 1)
            {
                _hasEnteredCling = false;
            }

            if (isOnGround && _playerRB.velocity.y == 0)
            {
                _hasEnteredJump = false;
            }

            if (!IsMeditating)
            {
                _hasEnteredMeditation = false;
            }

            if (!isDead)
            {
                _hasEnteredDeath = false;
            }
        }

        private void RequestAudio()
        {
            Clips clip;

            if (Mathf.Abs(_horizontalVelocity) > 0 && isOnGround)
            {
                clip = Clips.Move;
            }

            else if (_horizontalVelocity == 0 && IsMeditating && !_hasEnteredMeditation)
            {
                _hasEnteredMeditation = true;
                clip = Clips.Meditation;
            }

            else if (_playerRB.gravityScale == 0 && !_hasEnteredCling 
                && _currentState.Contains(cling.StateId()))
            {
                _hasEnteredCling = true;
                clip = Clips.Cling;
            }

            else if (_jumped && _playerRB.velocity.y > 0 && !_hasEnteredJump)
            {
                _hasEnteredJump = true;
                clip = Clips.Jump;
            }

            else if (isDead && !_hasEnteredDeath)
            {
                _hasEnteredDeath = true;
                clip = Clips.Dead;
            }

            else
            {
                return;
            }

            Utility.ActionHandler(Actions.Names.OnRequestAudio, clip, this);
        }

        private IEnumerator AudioRequestWithCooldown(float cooldown, Clips clip)
        {
            Utility.ActionHandler(Actions.Names.OnRequestAudio, clip, this);

            yield return new WaitForSeconds(cooldown);

            _hasEnteredGrappleShot = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + _config.groundRaycastOffset,
                transform.position + _config.groundRaycastOffset + Vector3.down
                * _config.groundRaycastLength);

            Gizmos.DrawLine(transform.position - _config.groundRaycastOffset,
                transform.position - _config.groundRaycastOffset + Vector3.down
                * _config.groundRaycastLength);

            Gizmos.color = Color.blue;

            Gizmos.DrawLine(transform.position,
                transform.position + Vector3.right * _config.wallRaycastLength);

            Gizmos.DrawLine(transform.position,
                transform.position + Vector3.left * _config.wallRaycastLength);
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            if(other.gameObject.CompareTag(Utility.SPIRITTAG)) 
            {
                IsMeditating = false;

                _turret.SetActive(false);

                Utility.ActionHandler(
                    Actions.Names.OnRequestAudio, Clips.SpiritTouch, this);
            }
        }
    }
}