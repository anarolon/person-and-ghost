using PersonAndGhost.Utils;
using UnityEngine;

namespace PersonAndGhost.Tools
{
    public class Tool : MonoBehaviour
    {
        [SerializeField] protected bool didSubscribe = false;
        [SerializeField] private bool isPickedUp = false;
        protected Transform obtainerPos = default;

        [SerializeField] protected LayerMask groundLayer = default;

        protected Rigidbody2D toolRb;
        protected PolygonCollider2D toolPolyCollider;

        // Testing variables
        [Header("Testing variables")]
        [SerializeField] private bool touchingGround = false;

        public bool IsPickedUp => isPickedUp; 

        private void Awake()
        {
            tag = "Tool";
        }

        protected virtual void Start()
        {
            didSubscribe = false;
            touchingGround = false;

            toolRb = GetComponent<Rigidbody2D>();
            toolPolyCollider = GetComponent<PolygonCollider2D>();

            isPickedUp = false;

            ZeroGravityEffect();
        }


        protected virtual void Update()
        {
            if (isPickedUp && obtainerPos != null)
            {
                transform.position = obtainerPos.position;
            }
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Person") && !didSubscribe)
            {
                Actions.OnToolPickup += ToolPickup;
                Actions.OnToolDrop += ToolDrop;

                didSubscribe = true;
            }

        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Person"))
            {
                if (didSubscribe)
                {
                    Actions.OnToolPickup -= ToolPickup;
                    Actions.OnToolDrop -= ToolDrop;
                    didSubscribe = false;
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 3) // Magic Number
            {
                ZeroGravityEffect();
            }
        }

        protected virtual void ToolAction(GameObject obtainer)
        {

        }

        protected virtual void ToolDrop(GameObject obtainer)
        {
            isPickedUp = false;
            obtainerPos = null;

            AddGravityEffect();

            Actions.OnToolActionUse -= ToolAction;
        }

        protected virtual void ToolPickup(GameObject obtainer)
        {
            isPickedUp = true;
            obtainerPos = obtainer.transform;

            ZeroGravityEffect();

            Actions.OnToolActionUse += ToolAction;
        }

        private void ZeroGravityEffect()
        {
            if (toolRb)
            {
                toolRb.gravityScale = 0;
                toolRb.velocity = Vector2.zero;
                toolPolyCollider.enabled = false;
            }
        }

        private void AddGravityEffect()
        {
            toolPolyCollider.enabled = true;
            toolRb.gravityScale = 0.5f;
        }
    }
}