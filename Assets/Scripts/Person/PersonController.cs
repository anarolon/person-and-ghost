using UnityEngine;
using UnityEngine.InputSystem;

public class PersonController : MonoBehaviour
{
    [Header("State Machine Fields")]
    private StateMachine movementSM;
    public IdleState idle;
    public MovementState movement;
    public JumpingState jumping;
    public ClingState cling;
    [SerializeField] string currentState; // Just for demonstration purposes currently

    [Header("Components")]
    private Rigidbody2D playerRB;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Movement Fields")]
    [SerializeField] private float movementAcceleration = 50f;
    [SerializeField] private float maxMoveSpeed = 12f;
    [SerializeField] private float linearDrag = 10f;
    private float horizontalVelocity;
    private bool changingDirection => (
        playerRB.velocity.x > 0f && horizontalVelocity < 0f)
        || (playerRB.velocity.x < 0f && horizontalVelocity > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float airLinearDrag = 2.5f;
    [SerializeField] private float fallMultiplier = 8f;
    [SerializeField] private float lowJumpFallMultiplier = 5f;
    private bool canJump = false;

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength = 1.2f;
    [SerializeField] private Vector3 groundRaycastOffset = new Vector3(0.5f, 0, 0);
    [SerializeField] private bool isOnGround = false;

    [Header("Wall Collision Variables")]
    [SerializeField] private float wallRaycastLength = 0.5f;
    [SerializeField] private bool isTouchingWall = false;


    [Header("Input System Variables")]
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;

    // PROPERTIES
    public StateMachine MovementSM { get => movementSM; }
    public bool CanJump { get => canJump; set => canJump = value; }
    public float GroundRayCastLength
    {
        get => groundRaycastLength; set => groundRaycastLength = value;
    }
    public bool IsOnGround { get => isOnGround; set => isOnGround = value; }
    public bool IsTouchingWall { get => isTouchingWall; }
    public bool IsMeditating { get; set; }
    public Vector2 MovementInput { get => movementInput; }
    public bool Jumped { get => jumped; }

    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        movementSM = new StateMachine();

        idle = new IdleState(this, movementSM);
        movement = new MovementState(this, movementSM);
        jumping = new JumpingState(this, movementSM);
        cling = new ClingState(this, movementSM);
        movementSM.Initialize(idle);
    }

    private void Update()
    {
        movementSM.CurrentState.HandleInput();

        movementSM.CurrentState.LogicUpdate();
        currentState = movementSM.CurrentState.ToString();
    }

    private void FixedUpdate()
    {
        movementSM.CurrentState.PhysicsUpdate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }


    public bool IsWalking()
    {
        return Mathf.Abs(playerRB.velocity.x) > 0;
    }

    public void ResetVelocity()
    {
        playerRB.velocity = Vector2.zero;
        playerRB.angularVelocity = 0;
    }

    public void Move()
    {
        CheckCollision();
        if (IsMeditating)
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        }
        else
        {
            MovePlayer(movementInput);
        }
        if (isOnGround)
        {
            ApplyLinearDrag();
        }
    }

    public void AirMove()
    {
        CheckCollision();
        MovePlayer(movementInput);
        ApplyAirLinearDrag();
        ApplyFallMultiplier();
    }

    public void Cling()
    {
        playerRB.gravityScale = 0;
        playerRB.velocity = Vector2.zero;
    }

    public void UnCling()
    {
        playerRB.gravityScale = 1;
    }



    private void MovePlayer(Vector2 movementInput)
    {
        horizontalVelocity = movementInput.x;

        playerRB.AddForce(new Vector2(horizontalVelocity, 0f) * movementAcceleration);

        if (Mathf.Abs(playerRB.velocity.x) > maxMoveSpeed)
        {
            playerRB.velocity = new Vector2(Mathf.Sign(playerRB.velocity.x)
                * maxMoveSpeed, playerRB.velocity.y);
        }
    }

    private void ApplyLinearDrag()
    {
        // TODO: Magic Number: 0.4f; make into a variable
        if (Mathf.Abs(horizontalVelocity) < 0.4f || changingDirection)
        {
            playerRB.drag = linearDrag;
        }
        else
        {
            playerRB.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        playerRB.drag = airLinearDrag;
    }

    public void Jump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
        playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

    }

    private void ApplyFallMultiplier()
    {
        if (playerRB.velocity.y < 0)
        {
            playerRB.gravityScale = fallMultiplier;
        }
        else if (playerRB.velocity.y > 0 && !jumped)
        {
            playerRB.gravityScale = lowJumpFallMultiplier;
        }
        else
        {
            playerRB.gravityScale = 1f;
        }
    }

    private void CheckCollision()
    {
        isOnGround = Physics2D.Raycast(transform.position + groundRaycastOffset,
            Vector2.down, groundRaycastLength, groundLayer)
            || Physics2D.Raycast(transform.position - groundRaycastOffset,
            Vector2.down, groundRaycastLength, groundLayer);

        isTouchingWall = Physics2D.Raycast(transform.position,
            Vector2.right, wallRaycastLength, wallLayer)
            || Physics2D.Raycast(transform.position,
            Vector2.left, wallRaycastLength, wallLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + groundRaycastOffset,
            transform.position + groundRaycastOffset + Vector3.down
            * groundRaycastLength);

        Gizmos.DrawLine(transform.position - groundRaycastOffset,
            transform.position - groundRaycastOffset + Vector3.down
            * groundRaycastLength);

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position,
            transform.position + Vector3.right * wallRaycastLength);

        Gizmos.DrawLine(transform.position,
            transform.position + Vector3.left * wallRaycastLength);

    }

}

