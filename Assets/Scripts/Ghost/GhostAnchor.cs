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
            Gizmos.DrawWireCube(AnchorTransformPosition,
                (AnchorRangeValue * 2) * Vector2.one);
        }

        private void UpdatePossession(bool isPossessing)
        {
            _isPossessing = isPossessing;
        }

        private void AdjustDistanceFromAnchor()
        {
            Vector2 positionDifference = AnchorTransformPosition -
                (Vector2)transform.position;

            if (Mathf.Abs(positionDifference.x) > AnchorRangeValue
                || Mathf.Abs(positionDifference.y) > AnchorRangeValue)
            {
                transform.position = AnchorTransformPosition;
            }
        }

        private void OnDisable()
        {
            Actions.OnPossessionTriggered -= UpdatePossession;
        }
    }
}