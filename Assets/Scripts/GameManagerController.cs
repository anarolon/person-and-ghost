using UnityEngine;
using UnityEngine.UI;
using PersonAndGhost.Utils;

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
            }

            else
            {
                //Debug.Log("Players Lost Room");

                _gameResultTextBox.text = _losingRoomMessage;
            }

            StartCoroutine(Utility.SceneHandler(hasWon, timeToWaitBeforeLoadingScene));
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
    }
}