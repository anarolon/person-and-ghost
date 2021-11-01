using PersonAndGhost.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    private GameObject _pauseMenuPanel;
    // TODO: Code a way to make sure _pauseFirstButton is set to an object
    [SerializeField] private GameObject _pauseFirstButton;

    private void Start()
    {
        _pauseMenuPanel = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        Actions.OnGamePause += HandleGamePause;
        // TODO: Subscribe to HandleGameUnPause as well and have GameManager call it if it is pressed when the pause menu is already active
    }
    private void OnDisable()
    {
        Actions.OnGamePause -= HandleGamePause;
    }

    private void HandleGamePause()
    {
        if (_pauseMenuPanel && !_pauseMenuPanel.activeSelf)
        {
            if (EventSystem.current)
            {
                EventSystem.current.SetSelectedGameObject(null);
                if (_pauseFirstButton)
                {
                    EventSystem.current.SetSelectedGameObject(_pauseFirstButton);
                }
            }
            
               
                    
            _pauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // PUBLIC METHODS FOR THE BUTTONS TO CALL
    public void HandleGameUnPause()
    {
        if (_pauseMenuPanel && _pauseMenuPanel.activeSelf)
        {
            _pauseMenuPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void RoomReset()
    {
        HandleGameUnPause();
        StopAllCoroutines(); // Just in case
        StartCoroutine(Utility.SceneHandler(false, 0));

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
