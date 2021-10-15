using UnityEngine;

[CreateAssetMenu]
public class PersonConfig : ScriptableObject
{

    [Header("Movement Fields")]
    public float movementAcceleration = 50f;
    public float maxMoveSpeed = 12f;
    public float linearDrag = 10f;

    [Header("Jump Variables")]
    public float jumpForce = 10f;
    public float airLinearDrag = 3f;
    public float fallMultiplier = 8f;
    public float lowJumpFallMultiplier = 5f;

    [Header("Ground Collision Variables")]
    public float groundRaycastLength = 0.6f;
    public Vector3 groundRaycastOffset = new Vector3(0.25f, 0, 0);

    [Header("Wall Collision Variables")]
    public float wallRaycastLength = 0.6f;

    [Header("Grapple Fields")]
    public float grappleDistance = 60;
    public float reachedGrapplepointDistance = 0.5f;
    public float grappleSpeedMin = 10f;
    public float grappleSpeedMax = 50f;
    public float grappleMultiplier = 50f;

}