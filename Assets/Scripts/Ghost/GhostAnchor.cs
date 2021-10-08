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

        public float AnchorRange { get; private set; } = 2.5f;

        // TODO: Make Anchor Range grow to a radius approximately the size of the entire camera view if anchor is meditating
        private float AnchorRangeValue => _anchor.IsMeditating ?
                AnchorRange * _anchorRangeGrowth : AnchorRange;

        private Vector2 AnchorTransformPosition => _anchor.transform.position;

        private void Awake()
        {
            if (!_config)
            {
                _config = ScriptableObject.CreateInstance<GhostConfig>();
            }

            AnchorRange = _config.anchorRange;
            _anchorRangeGrowth = _config.anchorRangeGrowth;
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
            //Gizmos.DrawWireCube(AnchorTransformPosition,
                //(AnchorRangeValue * 2) * Vector2.one);
            Gizmos.DrawWireSphere(AnchorTransformPosition, AnchorRangeValue);
        }

        private void UpdatePossession(bool isPossessing)
        {
            _isPossessing = isPossessing;
        }

        private void AdjustDistanceFromAnchor()
        {

            float distanceToAnchor = Vector3.Distance((Vector2)transform.position, AnchorTransformPosition);

            if (distanceToAnchor > AnchorRangeValue)
            {
                Vector2 distanceToAnchorVector = (Vector2)transform.position - AnchorTransformPosition;

                distanceToAnchorVector *= AnchorRangeValue / distanceToAnchor;
                transform.position = AnchorTransformPosition + distanceToAnchorVector;
            }
        }

        private void OnDisable()
        {
            Actions.OnPossessionTriggered -= UpdatePossession;
        }
    }
}