using PersonAndGhost.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PersonAndGhost
{
    public class PauseMenuController : MonoBehaviour
    {
        private GameObject _pauseMenuPanel;
        // TODO: Code a way to make sure _pauseFirstButton is set to an object
        [SerializeField] private GameObject _pauseFirstButton;
        
        private Button _currentButton = default;
        private GameObject _previousSelected = default;
        private EventSystem _eventSystem;

        private void Start()
        {
            _pauseMenuPanel = transform.GetChild(0).gameObject;
            _eventSystem = EventSystem.current;
        }

        private void OnEnable()
        {
            Actions.OnGamePause += HandleGamePause;
            // TODO: Subscribe to HandleGameUnPause as well 
            //and have GameManager call it if it is pressed 
            //when the pause menu is already active
        }
        private void OnDisable()
        {
            Actions.OnGamePause -= HandleGamePause;
        }

        private void Update()
        {
            GameObject currentSelected = _eventSystem.currentSelectedGameObject;

            if (_pauseMenuPanel.activeSelf)
            {
                if (_previousSelected == currentSelected)
                {
                    if (!_currentButton)
                    {
                        // The following assignment may slow performance because 
                        //  it is called each time a new button is selected.
                        _currentButton = currentSelected.GetComponent<Button>();
                    }
                }

                else
                { 
                    Utility.ActionHandler(Actions.Names.OnRequestAudio, Clips.UIMoving, this);

                    _previousSelected = currentSelected;
                    _currentButton = null;
                }

                if (_currentButton)
                {
                    _currentButton.onClick.AddListener(delegate
                    {
                        Utility.ActionHandler(Actions.Names.OnRequestAudio, Clips.UISelecting, this);
                    });
                }
            }
        }

        private void HandleGamePause()
        {
            if (_pauseMenuPanel && !_pauseMenuPanel.activeSelf)
            {
                if (_eventSystem)
                {
                    _eventSystem.SetSelectedGameObject(null);
                    if (_pauseFirstButton)
                    {
                        _eventSystem.SetSelectedGameObject(_pauseFirstButton);

                        _previousSelected = _eventSystem.currentSelectedGameObject;
                    }
                }
            
               
                    
                _pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
                Utility.ActionHandler(Actions.Names.OnRequestAudio, Clips.Pause, this);
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
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadSceneAsync(sceneIndex);

            if (Time.timeScale == 0)
            {
                HandleGameUnPause();
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
