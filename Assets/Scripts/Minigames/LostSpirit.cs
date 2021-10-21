using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSpirit : Spirit
{
    private float _frequency = 2.0f;
    private float _amplitude = 3.0f;
    private Vector2 _pos;
    private Vector2 _axis;

    protected override void Start() {
        base.Start();
        _speed = 0.25f;
        _pos = transform.position;
        _axis = transform.up;
    }

    private void FixedUpdate() {
        Vector2 lookDir =(Vector2) target.position - _spiritRb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        _spiritRb.rotation = angle;
        lookDir.Normalize();
        _pos += lookDir * Time.fixedDeltaTime;
        // zig zag and forward movement
        _spiritRb.MovePosition(_pos + _axis * _speed * Mathf.Sin(Time.time * _frequency) * _amplitude);
    }

    private void ZigZagMovement() {

    }
}
