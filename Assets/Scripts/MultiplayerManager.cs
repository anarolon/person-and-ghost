using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    [Header("Player 1 Fields")]
    [SerializeField] private GameObject _player1Prefab = default;
    [SerializeField] private Vector2 _player1Position = Vector2.one;

    [Header("Player 2 Fields")]
    [SerializeField] private GameObject _player2Prefab = default;
    [SerializeField] private Vector2 _player2Position = -Vector2.one;

    private void Start()
    {
        Transform player1 = PlayerInput.Instantiate
        (
            _player1Prefab,
            controlScheme: "KeyboardLeft",
            pairWithDevice: Keyboard.current
        ).gameObject.transform;

        Transform player2 = PlayerInput.Instantiate
            (
                _player2Prefab,
                controlScheme: "KeyboardRight",
                pairWithDevice: Keyboard.current
            ).gameObject.transform;

        player1.parent = this.gameObject.transform;
        player2.parent = this.gameObject.transform;

        player1.position = _player1Position;
        player2.position = _player2Position;
    }
}
