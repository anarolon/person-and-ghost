using PersonAndGhost.Utils;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    public Transform target;
    protected float _speed = 0.5f;
    protected Rigidbody2D _spiritRb;

    protected virtual void Start() 
    {
        _spiritRb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Person").GetComponent<Transform>();
    }

    public void Hit() 
    {
        Destroy(gameObject);
        Utility.ActionHandler(Actions.Names.OnRequestAudio, Clips.EnemyDeath, this);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(Equals(other.gameObject.tag, "Person")) 
        {
            Destroy(gameObject);
        }
    }
}
