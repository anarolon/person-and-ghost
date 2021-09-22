using UnityEngine.InputSystem;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [Header("Component Fields")]
    private Rigidbody2D _rb = default;

    [Header("Movement Fields")]
    private Vector2 _movementInput = Vector2.zero;
    [SerializeField] private float _movementAcceleration = 50f;
    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] private float _linearDrag = 10f;

    [Header("Anchor Fields")]
    [SerializeField] private float _anchorRBRange = 5;
    private Rigidbody2D _anchorRB = default;
    private PlayerController _anchorScript = default;
    private float _anchorRBRangeValue = 5;
    private Camera _mainCamera = default;

    [Header("Possession Fields")]
    private bool _isNearAMonster = false;
    private bool _isPossessing = false;
    private AIAgent _monster = default;
    private SpriteRenderer _ghostVanishes = default;
    private AIAgent _nearbyMonster;


    // Properties used for testing 
    public bool IsNearAMonster => _isNearAMonster;
    public float AnchorRBRange => _anchorRBRange;

    public AIAgent Monster
    {
        get => _monster;
        set => _monster = value;
    }

    public Rigidbody2D Anchor
    {
        get => _anchorRB;
        set => _anchorRB = value;
    }

    public bool IsPossessing
    {
        get => _isPossessing;
        set => _isPossessing = value;
    }

    public Vector2 MovementInput
    {
        get => _movementInput;
        set => _movementInput = value;
    }
    //

    private void Start()
    {
        _ghostVanishes = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;

        try
        {
            GameObject anchor = GameObject.FindGameObjectWithTag("Person");
            _anchorRB = anchor.GetComponent<Rigidbody2D>();
            _anchorScript = anchor.GetComponent<PlayerController>();
        }
        catch
        {
            GameObject anchor = new GameObject
            {
                name = "Anchor"
            };

            _anchorRB = anchor.AddComponent<Rigidbody2D>();
            _anchorScript = anchor.AddComponent<PlayerController>();

            _anchorRB.constraints = RigidbodyConstraints2D.FreezePositionY;

            Debug.LogWarning("GameObject With Tag Person Not Found. New GameObject named Anchor was created.");
        }
    }

    private void FixedUpdate()
    {
        _anchorRBRangeValue = _anchorScript.IsMeditating ? _anchorRBRange * 2 : _anchorRBRange;

        if (!_isPossessing)
        {
            _ghostVanishes.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            MoveGhost();
            AdjustDistanceFromAnchor();
        }
        else
        {
            _ghostVanishes.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            MoveMonster();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnPossession(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();
        bool triggered = context.action.triggered;

        if (triggered && _isNearAMonster)
        {
             _isPossessing = !_isPossessing;
            if(!_isPossessing) {
                //Debug.Log("Unpossessing Creature");
                _isNearAMonster = false;
                _monster.stateMachine.ChangeState(_monster.initialState);
            } else {
                //Debug.Log("Possessed Creature");
                _monster = _nearbyMonster;
                _monster.stateMachine.ChangeState(AIStateId.Possessed);
            }
        }
    }

    

    private void MoveGhost()
    {
        _rb.AddForce(_movementInput * _movementAcceleration);

        var velocity = _rb.velocity;
        if (Mathf.Abs(velocity.x) > _maxMoveSpeed || Mathf.Abs(velocity.y) > _maxMoveSpeed)
        {
            var x = Mathf.Sign(velocity.x) * _maxMoveSpeed;
            var y = Mathf.Sign(velocity.y) * _maxMoveSpeed;
            _rb.velocity = new Vector2(x, y);
        }

        _rb.drag = _linearDrag;
    }

    private void AdjustDistanceFromAnchor()
    {
        Vector2 anchorPosition = _anchorRB.position;
        Vector2 positionDifference = anchorPosition - _rb.position;

        if (Mathf.Abs(positionDifference.x) > _anchorRBRangeValue
            || Mathf.Abs(positionDifference.y) > _anchorRBRangeValue
            || !IsVisibleToCamera(_mainCamera, transform.position))
        {
            _rb.MovePosition(anchorPosition);
        }
    }

    private void MoveMonster()
    {
        _monster.movementBehaviour(_movementInput);
        _rb.MovePosition(_monster.rb.position);
    }

    public void OnMonsterJump(InputAction.CallbackContext context) {
        if(_isPossessing) {
            context.ReadValue<float>();
            _monster._isJumping = context.action.triggered;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Equals(other.gameObject.tag, "Monster"))
        {
            _isNearAMonster = true;
            _nearbyMonster = other.gameObject.GetComponent<AIAgent>();
        }
    }

        private void OnTriggerExit2D(Collider2D other)
    {
        if (Equals(other.gameObject.tag, "Monster"))
        {
            _isNearAMonster = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 from = _anchorRB.position;
        Vector3 to = _anchorRBRangeValue * 2 * Vector2.one;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(from, to);
    }

    //To be visible, the object most be between 0 and 1 for both X and Y positions
    private static bool IsVisibleToCamera(Camera mainCamera, Vector3 objectPosition)
    {
        Vector3 cameraVision = mainCamera.WorldToViewportPoint(objectPosition);
        return (cameraVision.x >= 0 && cameraVision.y >= 0)
                && (cameraVision.x <= 1 && cameraVision.y <= 1);
    }
}