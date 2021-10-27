using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PersonAndGhost.Utils;

public class GameManagerController : MonoBehaviour
{
    [Header("User Interface Fields")]
    [SerializeField] private string _winningMessage = "You Win!!!";
    [SerializeField] private string _losingMessage = "You Lose!!!";
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
        Actions.OnCollectableCollected += UpdateCollectableCount;
    }

    private void OnDisable()
    {
        Actions.OnRoomStateChange -= HandleRoomStateChange;
        Actions.OnCollectableCollected -= UpdateCollectableCount;
    }

    private void HandleRoomStateChange(bool hasWon)
    {
        if (hasWon)
        {
            //Debug.Log("Players Won Room");

            _gameResultTextBox.text = _winningMessage;
        }

        else
        {
            //Debug.Log("Players Lost Room");

            _gameResultTextBox.text = _losingMessage;
        }
    }

    private void UpdateCollectableCount(int amount)
    {
        _collectableTextBox.text = amount.ToString();
    }
}
