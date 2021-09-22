using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    [Header("Players Fields")]
    [SerializeField] private GameObject _personPrefab = default;
    [SerializeField] private GameObject _ghostPrefab = default;

    private void Start()
    {
        PlayerInput.Instantiate(_personPrefab, controlScheme:"KeyboardLeft", pairWithDevice: Keyboard.current);
        PlayerInput.Instantiate(_ghostPrefab, controlScheme:"KeyboardRight", pairWithDevice: Keyboard.current);
    }
}
