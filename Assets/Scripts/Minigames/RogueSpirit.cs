using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSpirit : Spirit
{
    protected override void Start() {
        base.Start();
        _speed = 1.0f;
    }

    private void FixedUpdate() {
        Vector2 lookDir =(Vector2) target.position - _spiritRb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        _spiritRb.rotation = angle;
        lookDir.Normalize();
        _spiritRb.MovePosition(_spiritRb.position + lookDir * _speed * Time.fixedDeltaTime);
    }
}
