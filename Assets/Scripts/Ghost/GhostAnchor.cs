using System.Collections.Generic;
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

        [Header("Boundary fields")]
        private bool hasExpandedBoundary = default;
        private Vector2 ghostBound = default;
        private Dictionary<string, float> _boundaries = default;

        public float AnchorRange { get; private set; } = 2.5f;
        public bool HasExpandedBoundary { get => hasExpandedBoundary; set => hasExpandedBoundary = value; }

        // TODO: Ask why make this a Property if its private?
        private Vector2 AnchorPosition => _anchor.transform.position;

        

        private void Awake()
        {
            _config ??= ScriptableObject.CreateInstance<GhostConfig>();

            AnchorRange = _config.anchorRange;
            _anchorRangeGrowth = _config.anchorRangeGrowth;

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
            _boundaries = GetViewportBoundaries();
        }


        private void FixedUpdate()
        {
            hasExpandedBoundary = _anchor.IsMeditating;
            if (!_isPossessing)
            {
                AdjustDistanceFromAnchor();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if (!hasExpandedBoundary) {
                Gizmos.DrawWireSphere(AnchorPosition, AnchorRange);
            }
            //else
            //{
            //    Gizmos.DrawWireCube(_camera.transform.position, new Vector3(2*_camera.orthographicSize * _camera.aspect, 2*_camera.orthographicSize, 0));
            //}
        }

        private void UpdatePossession(bool isPossessing, AIAgent monster)
        {
            _isPossessing = isPossessing;
        }

        private void AdjustDistanceFromAnchor()
        {

            if (!hasExpandedBoundary)
            {
                transform.position = CalculateAnchorBoundPosition();
            }

            transform.position = CalculateCameraBoundPosition();
            
        }

        public Vector2 CalculateAnchorBoundPosition()
        {
            float distanceToAnchor = Vector3.Distance((Vector2)transform.position, AnchorPosition);
            if (distanceToAnchor > AnchorRange)
            {
                Vector2 distanceToAnchorVector = (Vector2)transform.position - AnchorPosition;

                distanceToAnchorVector *= AnchorRange / distanceToAnchor;
                return AnchorPosition + distanceToAnchorVector;
            }
            else
            {
                return transform.position;
            }
        }

        private Dictionary<string, float> GetViewportBoundaries()
        {
            Dictionary<string, float> boundaries = new Dictionary<string, float>();

            boundaries.Add("left", _camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x
                + ghostBound.x);
            boundaries.Add("right", _camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x
                - ghostBound.x);
            boundaries.Add("bottom", _camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y
                + ghostBound.y);
            boundaries.Add("top", _camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y
                - ghostBound.y);

            return boundaries;
        }

        public Vector2 CalculateCameraBoundPosition()
        // TODO: Find out if there is a better place for this function to be in.
        {
            _boundaries = GetViewportBoundaries();

            Vector2 correctedPos = transform.position;
            correctedPos.x = Mathf.Clamp(correctedPos.x, _boundaries["left"], _boundaries["right"]);
            correctedPos.y = Mathf.Clamp(correctedPos.y, _boundaries["bottom"], _boundaries["top"]);
            return correctedPos;
        }


        private void OnDisable()
        {
            Actions.OnPossessionTriggered -= UpdatePossession;
        }
    }
}