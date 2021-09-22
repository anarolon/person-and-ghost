using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCollectableController : MonoBehaviour
{
    [SerializeField] private int collectableCount = 0;
    // Start is called before the first frame update
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
            Destroy(collision.gameObject);
        }
    }
}
