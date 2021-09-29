using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PersonAndGhost;

public class PersonCollectableController : MonoBehaviour
{
    [SerializeField] private int collectableCount = 0;
    private Text _textBox = default;

    // Start is called before the first frame update
    void Start()
    {
        collectableCount = 0;
        _textBox = FindObjectOfType<Canvas>().GetComponentsInChildren<Text>()[1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            Collectable collectable = collision.gameObject.GetComponent<Collectable>();
            collectableCount += collectable.worth;
            _textBox.text = collectableCount.ToString();
            Destroy(collision.gameObject);
        }
    }
}
