using UnityEngine;
using UnityEngine.UI;
using PersonAndGhost.Utils;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace PersonAndGhost
{
    public class GameManagerController : MonoBehaviour
    {
        public float timeToWaitBeforeLoadingScene = 2;

        [Header("User Interface Fields")]
        [SerializeField] private string _winningRoomMessage = "Room Won!!!";
        [SerializeField] private string _losingRoomMessage = "Room Lost!!!";
        [SerializeField] private string _winningFloorMessage = "Floor Won!!!";
        [SerializeField] private GameObject _gameUI = default;
        
        private Text _gameResultTextBox;
        private Text _collectableTextBox;

        public static GameManagerController instance = null;
        public static int solvedRoomCount = 0;

        [Header("Input System Variables")]
        private bool _pausePress = default;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }

            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            if (!_gameUI)
            {
                _gameUI = FindObjectOfType<Canvas>().gameObject;

                Debug.LogWarning("Game UI field was null.");
            }

            Text[] uiTextBoxes = _gameUI.GetComponentsInChildren<Text>();

            foreach(Text text in uiTextBoxes)
            {
                string name = text.name;

                if (name == "GameStateText")
                {
                    _gameResultTextBox = text;
                }

                else if (name == "CollectablesAmountText")
                {
                    _collectableTextBox = text;
                }
            }
        }

        private void OnEnable()
        {
            Actions.OnRoomStateChange += HandleRoomStateChange;
            Actions.OnFloorStateChange += HandleFloorStateChange;
            Actions.OnCollectableCollected += UpdateCollectableCount;
        }
        
        private void OnDisable()
        {
            Actions.OnRoomStateChange -= HandleRoomStateChange;
            Actions.OnFloorStateChange -= HandleFloorStateChange;
            Actions.OnCollectableCollected -= UpdateCollectableCount;
        }

        // **************PAUSE MENU METHOD
        public void OnPauseGame(InputAction.CallbackContext context)
        {
            _pausePress = context.action.triggered;
            if (_pausePress)
            {
                Actions.OnGamePause();
            }
        }
        // PAUSE MENU METHOD*****************

        private void HandleRoomStateChange(bool hasWon)
        {
            if (hasWon)
            {
                //Debug.Log("Players Won Room");

                _gameResultTextBox.text = _winningRoomMessage;
                solvedRoomCount++;

                Time.timeScale = 0;
            }

            else
            {
                //Debug.Log("Players Lost Room");

                _gameResultTextBox.text = _losingRoomMessage;
            }

            StartCoroutine(LoadScene(hasWon, timeToWaitBeforeLoadingScene));
        }

        private void HandleFloorStateChange(bool hasWon)
        {
            if (hasWon)
            {
                //Debug.Log("Players Won Floor");

                _gameResultTextBox.text = _winningFloorMessage;

                Time.timeScale = 0;
            }
        }

        private void UpdateCollectableCount(int amount)
        {
            _collectableTextBox.text = amount.ToString();
        }

        private IEnumerator LoadScene(bool hasWon, float timeToWaitBeforeLoadingScene)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            yield return new WaitForSecondsRealtime(timeToWaitBeforeLoadingScene);

            if (hasWon)
            {
                Debug.Log(solvedRoomCount);
                if (solvedRoomCount % 3 == 0)
                {
                    Debug.Log("Floor Complete");
                    Actions.OnFloorStateChange(hasWon);
                }

                if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {

                    Time.timeScale = 1;

                    SceneManager.LoadSceneAsync(sceneIndex + 1);
                }
                else
                {
                    Application.Quit();
                }
            }

            else
            {
                Debug.Log("Game Over");
                SceneManager.LoadSceneAsync(sceneIndex);
            }
        }
    }
}