using UnityEngine;
using PersonAndGhost.Utils;

namespace PersonAndGhost
{
    public class Goal : MonoBehaviour
    {
        [Header("Players Tag to Collide with Goal")]
        [SerializeField] private string _leftPlayerTag = "Person";
        [SerializeField] private string _rightPlayerTag = "Ghost";
        private bool _isCollidingWithLeftPlayer = false;
        private bool _isCollidingWithRightPlayer = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            string collidingTag = collision.gameObject.tag;

            if (collidingTag == _leftPlayerTag)
            {
                _isCollidingWithLeftPlayer = true;
            }

            else if (collidingTag == _rightPlayerTag)
            {
                _isCollidingWithRightPlayer = true;
            }

            if (_isCollidingWithLeftPlayer && _isCollidingWithRightPlayer)
            {
                Actions.OnPuzzleWin();
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            string collidingTag = collision.gameObject.tag;

            if (collidingTag == _leftPlayerTag)
            {
                _isCollidingWithLeftPlayer = false;
            }

            else if (collidingTag == _rightPlayerTag)
            {
                _isCollidingWithRightPlayer = false;
            }
        }
    }
}