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
        var player1PlayerInput = PlayerInput.Instantiate(_player1Prefab,
                                                       controlScheme: "KeyboardLeft",
                                                       pairWithDevice: Keyboard.current);
        var player2PlayerInput = PlayerInput.Instantiate(_player2Prefab, 
                                                       controlScheme: "KeyboardRight", 
                                                       pairWithDevice: Keyboard.current);

        player1PlayerInput.gameObject.transform.position = _player1Position;
        player2PlayerInput.gameObject.transform.position = _player2Position;
    }
}
