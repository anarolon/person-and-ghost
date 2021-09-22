using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    [Header(" X-axis Movement Fields")]
    public float _movementAcceleration = 12.5f;
    public float _maxMoveSpeed = 3;
    public float _linearDrag = 2;
    public float _changeDirectionFrequency = 2;

    [Header("Idle Behavior Fields")]
    public float _doIdleFrequency = 10f;

    [Header("Vertical Movement Fields")]
    public float _jumpForce = 10f;
    public float _airLinearDrag = 3.75f;
    public float _fallMultiplier = 12f;
    public float _lowJumpFallMultiplier = 7.5f;
    public float _doJumpFrequency = 5f;
}
