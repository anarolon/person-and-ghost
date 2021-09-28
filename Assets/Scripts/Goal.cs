using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [Header("Players Tag to Collide with Goal")]
    [SerializeField] private string _player1Tag = "Person";
    [SerializeField] private string _player2Tag = "Ghost";
    private bool _isCollidingWithPlayer1 = false;
    private bool _isCollidingWithPlayer2 = false;

    [Header("User Interface Fields")]
    [SerializeField] private Canvas _gameUI = default;
    [SerializeField] private string _winningMessage = "You Win!!!";
    private Text _textBox = default;

    private void Start()
    {
        _textBox = _gameUI.gameObject.GetComponentInChildren<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collidingTag = collision.gameObject.tag;

        if (collidingTag == _player1Tag)
        {
            _isCollidingWithPlayer1 = true;
        }
        else if (collidingTag == _player2Tag)
        {
            _isCollidingWithPlayer2 = true;
        }
        if (_isCollidingWithPlayer1 && _isCollidingWithPlayer2)
        {
            _textBox.text = _winningMessage;
            Destroy(this.gameObject);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string collidingTag = collision.gameObject.tag;

        if (collidingTag == _player1Tag)
        {
            _isCollidingWithPlayer1 = false;
        }
        else if (collidingTag == _player2Tag)
        {
            _isCollidingWithPlayer2 = false;
        }
    }
}
