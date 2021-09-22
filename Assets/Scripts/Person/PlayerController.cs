using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    [Header("Components")]
    private Rigidbody2D playerRB = default;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer = default;
    [SerializeField] private LayerMask wallLayer = default;

    [Header("Movement Variables")]
    [SerializeField] private float movementAcceleration = 50f;
    [SerializeField] private float maxMoveSpeed = 12f;
    [SerializeField] private float linearDrag = 10f;
    private float horizontalVelocity;
    private bool changingDirection => (playerRB.velocity.x > 0f && horizontalVelocity < 0f) || (playerRB.velocity.x < 0f && horizontalVelocity > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float airLinearDrag = 2.5f;
    [SerializeField] private float fallMultiplier = 8f;
    [SerializeField] private float lowJumpFallMultiplier = 5f;

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private Vector3 groundRaycastOffset;
    [SerializeField] private bool onGround;

    [Header("Wall Collision Variables")]
    [SerializeField] private bool onWall;
    
    private bool canJump => isJumping && onGround; 
    private bool isJumping = false;
    private bool canWallJump = false;

    [Header("Input System Variables")]
    private Vector2 movementInput = Vector2.zero;
  
    //Properties
    public bool IsMeditating { get; set; }
    public Vector2 MovementInput
    {
        get => movementInput;
        set => movementInput = value;
    }
    public bool IsJumping
    {
        get => isJumping;
        set => isJumping = value;
    }

    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        IsMeditating = false;   
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        isJumping = context.action.triggered;
    }

    private void Update()
    {
        if (canJump && !IsMeditating) Jump();
        if (canWallJump) WallJump();
    }

    void FixedUpdate()
    {
        CheckCollision();
        if (!IsMeditating) MovePlayer(movementInput);
        
        if (onGround) // For some reason onGround variable is being set to True a short while after jumping into a bottomless pit for a small amount of time;
        {
            ApplyLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            ApplyFallMultiplier();
        }
    }

    //TESTING
    public void MovePlayer(Vector2 movementInput)
    {
        horizontalVelocity = movementInput.x;
        playerRB.AddForce(new Vector2(horizontalVelocity, 0f) * movementAcceleration);

        if (Mathf.Abs(playerRB.velocity.x) > maxMoveSpeed)
        {
            playerRB.velocity = new Vector2(Mathf.Sign(playerRB.velocity.x) * maxMoveSpeed, playerRB.velocity.y);
        }
    }

    private void ApplyLinearDrag()
    {
        if (Mathf.Abs(horizontalVelocity) < 0.4f || changingDirection) // Magic Number, make into a variable
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

    //TESTING
    public void Jump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
        playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //Make the added force an impulse

        canWallJump = false;
    }

    public void WallJump()
    {
        Vector2 jumpDirection = Vector2.up * jumpForce + Vector2.left * jumpForce / 4; //Temporary, in actuality will have to change direction depending on which side the wall is.

        playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
        playerRB.AddForce(jumpDirection, ForceMode2D.Impulse); //Make the added force an impulse

        canWallJump = false;
    }

    private void ApplyFallMultiplier()
    {
        if (playerRB.velocity.y < 0)
        {
            playerRB.gravityScale = fallMultiplier;
        }
        else if (playerRB.velocity.y > 0 && !isJumping)
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
        onGround = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer)
            || Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer);

        onWall = Physics2D.Raycast(transform.position, Vector2.right, groundRaycastLength, wallLayer)
            || Physics2D.Raycast(transform.position, Vector2.left, groundRaycastLength, wallLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + groundRaycastOffset, transform.position + groundRaycastOffset + Vector3.down * groundRaycastLength);

        Gizmos.DrawLine(transform.position - groundRaycastOffset, transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * groundRaycastLength);

        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * groundRaycastLength);

    }

    public bool IsOnGround()
    {
        return onGround;
    }

    public bool IsOnWall()
    {
        return onWall;
    }

    public void SetWallJump(bool newCanWallJump)
    {
        canWallJump = newCanWallJump;
    }

}
