using UnityEngine;
using PersonAndGhost.Utils;
using PersonAndGhost.Person;

namespace PersonAndGhost.Ghost
{
    public class GhostAnchor : MonoBehaviour
    {
        [SerializeField] private GhostConfig _config = default;
        private float _anchorRangeGrowth = 2;
        private PersonMovement _anchor = default;
        private bool _isPossessing = false;
        private Camera _camera;

        [Header("Ghost bounds")]
        private Vector2 ghostBound = default;

        public float AnchorRange { get; private set; } = 2.5f;


        // TODO: Ask why make this a Property if its private?
        private Vector2 AnchorTransformPosition => _anchor.transform.position;

        private void Awake()
        {
            _config ??= ScriptableObject.CreateInstance<GhostConfig>();

            AnchorRange = _config.anchorRange;
            _anchorRangeGrowth = _config.anchorRangeGrowth;

            // TODO: Find more efficient way to get the cinemachine camera
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            Actions.OnPossessionTriggered += UpdatePossession;
        }

        private void Start()
        {
            _anchor = FindObjectOfType<PersonMovement>();

            if (!_anchor)
            {
                GameObject anchor = new GameObject("Anchor");
                anchor.AddComponent<Rigidbody2D>().constraints =
                    RigidbodyConstraints2D.FreezePositionY;
                _anchor = anchor.AddComponent<PersonMovement>();
            }

            ghostBound = GetComponent<SpriteRenderer>().bounds.size / 2;
        }

        private void FixedUpdate()
        {
            if (!_isPossessing)
            {
                AdjustDistanceFromAnchor();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if (!_anchor.IsMeditating) {
                Gizmos.DrawWireSphere(AnchorTransformPosition, AnchorRange);
            }
            //else
            //{
            //    Gizmos.DrawWireCube(_camera.transform.position, new Vector3(2*_camera.orthographicSize * _camera.aspect, 2*_camera.orthographicSize, 0));
            //}
        }

        private void UpdatePossession(bool isPossessing)
        {
            _isPossessing = isPossessing;
        }

        private void AdjustDistanceFromAnchor()
        {

            if (!_anchor.IsMeditating)
            {
                float distanceToAnchor = Vector3.Distance((Vector2)transform.position, AnchorTransformPosition);
                if (distanceToAnchor > AnchorRange)
                {
                    Vector2 distanceToAnchorVector = (Vector2)transform.position - AnchorTransformPosition;

                    distanceToAnchorVector *= AnchorRange / distanceToAnchor;
                    transform.position = AnchorTransformPosition + distanceToAnchorVector;
                }
            }

            // TODO: Find out if it is necessary to have this be in this method or in Start
            float leftBound = _camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x
                + ghostBound.x;
            float rightBound = _camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x
                - ghostBound.x;
            float bottomBound = _camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y
                + ghostBound.y;
            float topBound = _camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y
                - ghostBound.y;

            Vector2 correctedPos = transform.position;
            correctedPos.x = Mathf.Clamp(correctedPos.x, leftBound, rightBound);
            correctedPos.y = Mathf.Clamp(correctedPos.y, bottomBound, topBound);
            transform.position = correctedPos;
            
        }

        private void OnDisable()
        {
            Actions.OnPossessionTriggered -= UpdatePossession;
        }
    }
}