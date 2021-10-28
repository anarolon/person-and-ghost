using UnityEngine;
using UnityEngine.UI;
using PersonAndGhost.Utils;
using System.Collections;
using UnityEngine.SceneManagement;

namespace PersonAndGhost
{
    public class GameManagerController : MonoBehaviour
    {
        public float timeToWaitBeforeLoadingScene = 2;

        [Header("User Interface Fields")]
        [SerializeField] private string _winningRoomMessage = "Room Won!!!";
        [SerializeField] private string _losingRoomMessage = "Room Lost!!!";
        [SerializeField] private string _winningFloorMessage = "Floor Won!!!";
        [SerializeField] private Text _gameResultTextBox = default;
        [SerializeField] private Text _collectableTextBox = default;

        public static GameManagerController instance = null;
        public static int solvedRoomCount = 0;

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
            if (_gameResultTextBox == null || _collectableTextBox == null)
            {
                Canvas uiCanvas = FindObjectOfType<Canvas>();
                Text[] uiTextBoxes = uiCanvas.GetComponentsInChildren<Text>();

                for (int i = 0; i < uiTextBoxes.Length; i++)
                {
                    if (uiTextBoxes[i].name == "GameStateText")
                    {
                        _gameResultTextBox ??= uiTextBoxes[i];
                    }
                    else if (uiTextBoxes[i].name == "CollectablesAmountText")
                    {
                        _collectableTextBox ??= uiTextBoxes[i];
                    }
                }

                Debug.LogWarning("Either Game Result Text Box or Collectable Text Box are " +
                    "null.");
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
                    Actions.OnFloorStateChange(hasWon);
                }

                // TODO: Differentiate between going to the next room and going to the next floor
                if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {

                    Time.timeScale = 1;

                    SceneManager.LoadSceneAsync(sceneIndex + 1);
                }

                // TODO: Close game on last scene
                else
                {
                    Application.Quit();
                }
            }

            else
            {
                SceneManager.LoadSceneAsync(sceneIndex);
            }
        }
    }
}