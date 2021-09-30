using UnityEngine;
using UnityEngine.UI;

namespace PersonAndGhost
{
    public class Goal : MonoBehaviour
    {
        [Header("Players Tag to Collide with Goal")]
        [SerializeField] private string _player1Tag = "Person";
        [SerializeField] private string _player2Tag = "Ghost";
        private bool _isCollidingWithPlayer1 = false;
        private bool _isCollidingWithPlayer2 = false;

        private void Start()
        {
            
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
                Actions.OnPuzzleWin();
                Destroy(this.gameObject);
                
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
}