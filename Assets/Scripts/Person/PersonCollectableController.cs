using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PersonAndGhost;

public class PersonCollectableController : MonoBehaviour
{
    [SerializeField] private int collectableCount = 0;

    void Start()
    {
        collectableCount = 0;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            Collectable collectable = collision.gameObject.GetComponent<Collectable>();
            collectableCount += collectable.worth;
            Actions.OnCollectableCollected(collectableCount);
            Destroy(collision.gameObject);
        }
    }
}
