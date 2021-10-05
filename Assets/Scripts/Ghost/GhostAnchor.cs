using UnityEngine;
using PersonAndGhost.Utils;
using System.Collections;

namespace PersonAndGhost.Ghost
{
    public class GhostAnchor : MonoBehaviour
    {
        [Header("Anchor Fields")]
        [SerializeField] private float _anchorRange = 2.5f;
        [SerializeField] private float _anchorRangeGrowth = 2;
        private PlayerController _anchor = default;

        private bool _isPossessing = false;

        public float AnchorRange => _anchorRange;

        private float AnchorRangeValue => _anchor.IsMeditating ?
                _anchorRange * _anchorRangeGrowth : _anchorRange;

        private Vector2 AnchorTransformPosition => _anchor.transform.position;

        private void Start()
        {
            _anchor = FindObjectOfType<PlayerController>();

            if (!_anchor)
            {
                GameObject anchor = new GameObject
                (
                    name = "Anchor"
                );

                anchor.AddComponent<Rigidbody2D>().constraints =
                    RigidbodyConstraints2D.FreezePositionY;
                _anchor = anchor.AddComponent<PlayerController>();

                Debug.LogWarning("Player Controller was not added in the inspector.");
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

        private void OnEnable()
        {
            Actions.OnPossessionTriggered += UpdatePossession;
        }

        private void OnDisable()
        {
            Actions.OnPossessionTriggered -= UpdatePossession;
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
    }
}