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
        Actions.OnPuzzleWin += HandlePuzzleWin;
        Actions.OnPuzzleFail += HandlePuzzleFail;
        Actions.OnCollectableCollected += UpdateCollectableCount;
    }

    private void OnDisable()
    {
        Actions.OnPuzzleWin -= HandlePuzzleWin;
        Actions.OnPuzzleFail -= HandlePuzzleFail;
        Actions.OnCollectableCollected -= UpdateCollectableCount;
    }

    private void HandlePuzzleWin()
    {
        Debug.Log("Player Won");
        _gameResultTextBox.text = _winningMessage;

        Time.timeScale = 0;
    }

    private void HandlePuzzleFail()
    {
        Debug.Log("Player Lost");
        _gameResultTextBox.text = _losingMessage;
        
    }

    private void UpdateCollectableCount(int amount)
    {
        _collectableTextBox.text = amount.ToString();
    }
}
