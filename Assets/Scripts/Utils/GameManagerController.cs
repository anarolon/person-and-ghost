using PersonAndGhost.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        [Header("Icon Fields")]
        private Image _toolIcon;
        private Image _stolenActionIcon;
        [SerializeField] Sprite emptySprite = default;
        [SerializeField] Sprite birdActionSprite = default;
        [SerializeField] Sprite bigboyActionSprite = default;
        [SerializeField] Sprite buffboyActionSprite = default;
        [SerializeField] Sprite frogActionSprite = default;
        [SerializeField] Sprite climbingGauntletSprite = default;
        [SerializeField] Sprite grapplingHookSprite = default;

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

            Image[] uiIcons = _gameUI.GetComponentsInChildren<Image>();
            foreach(Image icon in uiIcons)
            {
                string name = icon.name;
                if( name == "Tool Icon")
                {
                    _toolIcon = icon.GetComponent<Image>();
                    _toolIcon.sprite = emptySprite ? emptySprite : null;
                }
                else if( name == "Stolen Action Icon")
                {
                    _stolenActionIcon = icon.GetComponent<Image>();
                    _stolenActionIcon.sprite = emptySprite ? emptySprite : null;
                }
            }

        }

        private void OnEnable()
        {
            Actions.OnRoomStateChange += HandleRoomStateChange;
            Actions.OnFloorStateChange += HandleFloorStateChange;
            Actions.OnCollectableCollected += UpdateCollectableCount;

            Actions.OnToolPickup += ToolIconChange;
            Actions.OnToolDrop += ToolIconRemove;

            Actions.OnPossessionTriggered += StolenActionIconChange;


        }
        
        private void OnDisable()
        {
            Actions.OnRoomStateChange -= HandleRoomStateChange;
            Actions.OnFloorStateChange -= HandleFloorStateChange;
            Actions.OnCollectableCollected -= UpdateCollectableCount;

            Actions.OnToolPickup -= ToolIconChange;
            Actions.OnToolDrop -= ToolIconRemove;

            Actions.OnPossessionTriggered -= StolenActionIconChange;
        }

        //*****Tool Icon CHANGES
        private void ToolIconChange(GameObject obtainer, GameObject tool)
        {
            if (tool.name == "Grappling Hook")
            {
                _toolIcon.sprite = grapplingHookSprite ? grapplingHookSprite : null;
            }
            else if(tool.name == "Climbing Gauntlet")
            {
                _toolIcon.sprite = climbingGauntletSprite ? climbingGauntletSprite : null;
            }
        }

        private void ToolIconRemove(GameObject obtainer, GameObject tool)
        {
            _toolIcon.sprite = emptySprite ? emptySprite : null;
        }

        //**Stolen Action Icon Changes
        private void StolenActionIconChange(bool isPossessing, AIAgent monster)
        {
            if (isPossessing && _stolenActionIcon)
            {
                if (monster.name.Contains("Bird"))
                {
                    _stolenActionIcon.sprite = birdActionSprite ? birdActionSprite : null;
                }
                else if (monster.name.Contains("BigBoy"))
                {
                    _stolenActionIcon.sprite = bigboyActionSprite ? bigboyActionSprite : null;
                }
                else if (monster.name.Contains("BuffBoy"))
                {
                    _stolenActionIcon.sprite = buffboyActionSprite ? buffboyActionSprite : null;
                }
                else if (monster.name.Contains("Frog"))
                {
                    _stolenActionIcon.sprite = frogActionSprite ? frogActionSprite : null;
                }
            }
            else
            {
                _stolenActionIcon.sprite = emptySprite ? emptySprite : null;
            }
            
        }

        public void OnPauseGame(InputAction.CallbackContext context)
        {
            _pausePress = context.action.triggered;
            if (_pausePress)
            {
                Actions.OnGamePause();
            }
        }

        private void HandleRoomStateChange(bool hasWon)
        {
            if (hasWon)
            {
                //Debug.Log("Players Won Room");

                _gameResultTextBox.text = _winningRoomMessage;
                solvedRoomCount++;

                //Time.timeScale = 0;
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
            Utility.ActionHandler(Actions.Names.OnRequestAudio, Clips.Collectable, this);
        }

        private IEnumerator LoadScene(bool hasWon, float timeToWaitBeforeLoadingScene)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            yield return new WaitForSecondsRealtime(timeToWaitBeforeLoadingScene);

            if (hasWon)
            {
                //Debug.Log(solvedRoomCount);
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
                //Debug.Log("Game Over");
                SceneManager.LoadSceneAsync(sceneIndex);
            }
        }
    }
}