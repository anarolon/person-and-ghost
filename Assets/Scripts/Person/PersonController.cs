using UnityEngine;
using UnityEngine.InputSystem;

public class PersonController : MonoBehaviour
{

    [Header("State Machine Fields")]
    private StateMachine _movementSM;
    public IdleState idle;
    public MovementState movement;
    public JumpingState jumping;
    public FallingState falling;
    public MeditatingState meditate;
    public ClingState cling;
    [Tooltip("Currently only being used to visualize current player state")]
    [SerializeField] private string _currentState = default;

    [Header("Cached Components and Fields")]
    private Rigidbody2D _playerRB = default;
    [SerializeField] private PersonConfig _config = default;

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

    // PROPERTIES
    public StateMachine MovementSM { get => _movementSM;}
    public bool CanJump { get => _canJump; set => _canJump = value;}
    public bool IsOnGround { get => isOnGround;}
    public bool IsTouchingWall { get => isTouchingWall;}
    public bool IsMeditating { get; set;}
    public Vector2 MovementInput { get => _movementInput;}
    public bool Jumped { get => _jumped;}


    

    void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _groundLayer = LayerMask.GetMask("Ground");
        _wallLayer = LayerMask.GetMask("Wall");
        _config ??= ScriptableObject.CreateInstance<PersonConfig>();
    }

    private void Start()
    {
        _movementSM = new StateMachine();

        idle = new IdleState(this, _movementSM);
        movement = new MovementState(this, _movementSM);
        jumping = new JumpingState(this, _movementSM);
        falling = new FallingState(this, _movementSM);
        meditate = new MeditatingState(this, _movementSM);
        cling = new ClingState(this, _movementSM);
        
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
        _movementSM.CurrentState.PhysicsUpdate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumped = context.action.triggered;
    }


    public bool IsWalking()
    {
        return Mathf.Abs(_playerRB.velocity.x) > 0;
    }

    public void ResetVelocity()
    {
        _playerRB.velocity = Vector2.zero;
        _playerRB.angularVelocity = 0;
    }

    public void Move()
    {
        CheckCollision();
        // TODO: Do we need this meditation check here?
        if (IsMeditating)
        {
            _playerRB.velocity = new Vector2(0, _playerRB.velocity.y);
        }
        else
        {
            MovePlayer(_movementInput);
        }
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



    private void MovePlayer(Vector2 movementInput)
    {
        _horizontalVelocity = movementInput.x;

        _playerRB.AddForce(new Vector2(_horizontalVelocity, 0f) * _config.movementAcceleration);

        if (Mathf.Abs(_playerRB.velocity.x) > _config.maxMoveSpeed)
        {
            _playerRB.velocity = new Vector2(Mathf.Sign(_playerRB.velocity.x)
                * _config.maxMoveSpeed, _playerRB.velocity.y);
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
        isOnGround = Physics2D.Raycast(transform.position + _config.groundRaycastOffset,
            Vector2.down, _config.groundRaycastLength, _groundLayer)
            || Physics2D.Raycast(transform.position - _config.groundRaycastOffset,
            Vector2.down, _config.groundRaycastLength, _groundLayer);

        isTouchingWall = Physics2D.Raycast(transform.position,
            Vector2.right, _config.wallRaycastLength, _wallLayer)
            || Physics2D.Raycast(transform.position,
            Vector2.left, _config.wallRaycastLength, _wallLayer);
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

}

