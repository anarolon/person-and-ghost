using UnityEngine;

namespace PersonAndGhost.Ghost
{
    [CreateAssetMenu()]
    public class GhostConfig : ScriptableObject
    {
        // Possession Configuration
        [Header("Scale Field")]
        public Vector3 scale = Vector3.one * 0.5f;

        [Header("Sprite Renderer Fields")]
        public int sortingOrder = 10;
        public Sprite sprite = default;
        public Color color = new Color(40.0f / 255, 40.0f / 255, 40.0f / 255);

        [Header("Capsule Collider 2D Fields")]
        public bool isTrigger = true;
        public Vector2 size = new Vector2(1, 2);

        [Header("Anchor Fields")]
        public float anchorRange = 2.5f;
        public float anchorRangeGrowth = 2;

        [Header("Movement Fields")]
        public float movementAcceleration = 50f;
        public float maxMoveSpeed = 12f;
        public float linearDrag = 10f;

        [Header("Rigidbody 2D Fields")]
        public float gravityScale = 0;
        public CollisionDetectionMode2D collisionDetectionMode 
            = CollisionDetectionMode2D.Continuous;
        public RigidbodyConstraints2D constraints 
            = RigidbodyConstraints2D.FreezeRotation;
    }
}