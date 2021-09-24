using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour, ITool
{
    
    [SerializeField] protected bool onPlayer;
    protected PlayerController person;
    protected Rigidbody2D toolRb;
    protected Collider2D toolCollider;

    public bool OnPlayer {
        get => onPlayer;
        set => onPlayer = value;
    }

    void Start()
    {
        onPlayer = false;
        toolRb = GetComponent<Rigidbody2D>();
        toolCollider = GetComponent<Collider2D>();
        ZeroGravityEffect();
    }

    void Update()
    {
        if (onPlayer)
        {
            transform.position = person.gameObject.transform.position;
        }
    }

    public virtual void Action()
    {
        throw new System.NotImplementedException();
    }

    public virtual void GetDropped()
    {
        onPlayer = false;
        AddGravityEffect();
    }

    public virtual void GetPickedUp(PlayerController player)
    {
        onPlayer = true;
        person = player;
        ZeroGravityEffect();
    }

    private void ZeroGravityEffect() {
        toolRb.gravityScale = 0;
        toolRb.velocity = new Vector2(0f, toolRb.velocity.y);
        toolCollider.isTrigger = true;
    }

    private void AddGravityEffect() {
        toolRb.gravityScale = 1;
        toolCollider.isTrigger = false;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(Equals(other.gameObject.tag, "Ground")) {
            ZeroGravityEffect();
        }
    }
}
