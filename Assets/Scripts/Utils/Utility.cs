using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using PersonAndGhost.Ghost;
using System.Collections;
using UnityEngine.SceneManagement;

namespace PersonAndGhost.Utils
{
    public static class Utility
    {
        public const string MAINCAMERATAG = "MainCamera";
        public const string MONSTERTAG = "Monster";
        public const string SPIRITTAG = "Spirit";
        public const string LEFTPLAYERTAG = "Person";
        public const string RIGHTPLAYERTAG = "Ghost";
        public const string GHOSTLYINNVASIONTAG = "GhostlyInvasion";
        public const string LEFTCONTROLSCHEME = "KeyboardLeft";
        public const string RIGHTCONTROLSCHEME = "KeyboardRight";
        public const string GAMEUIPREFABPATH = "Prefabs/User Interface";
        public const string GAMEMANAGERPREFABPATH = "Prefabs/GameManager";
        public const string LEFTPLAYERPREFAB = "Prefabs/Person";
        public const string RIGHTPLAYERPREFABPATH = "Prefabs/Ghost";
        public const string BIRDPREFABPATH = "Prefabs/Bird";
        public const string CLAWEDBIRDPREFABPATH = "Prefabs/Bird (Clawed)";
        public const string BUFFBOYPREFABPATH = "Prefabs/BuffBoy";
        public const string BIGBOYPREFABPATH = "Prefabs/BigBoy";
        public const string GROUNDPREFABPATH = "Prefabs/Ground";
        public const string WALLPREFABPATH = "Prefabs/Wall";
        public const string BREAK_PLATFORM_PREFAB_PATH = "Prefabs/BreakablePlatform";
        public const string BREAK_WALL_PREFAB_PATH = "Prefabs/BreakableWall";
        public const string CRATE_PREFAB_PATH = "Prefabs/Crate";
        public const string LINEPREFABPATH = "Prefabs/Line";
        public const string CLIMBINGGAUNTLETPREFABPATH = "Prefabs/Climbing Gauntlet";
        public const string GRAPPLINGHOOKPREFABPATH = "Prefabs/Grappling Hook";
        public const string HATCHDOORPREFAB = "Prefabs/PressurePlateDoor_Hatch";
        public const string NORMALDOORPREFAB = "Prefabs/PressurePlateDoor_Normal";
        public const string CAPSULESPRITE = "Sprites/Capsule";
        public const string CIRCLESPRITEPATH = "Sprites/Circle";
        public const string DIAMONDSPRITEPATH = "Sprites/Diamond";
        public const string HEXAGONSPRITEPATH = "Sprites/Hexagon";
        public const string SQUARESPRITEPATH = "Sprites/Square";
        public const string CONTROLLERPREFAB = "Actions/Controller";
        public const string PLAYERACTIONMAP = "Player";
        public const string MOVEMENTACTION = "Movement";
        public const string JUMPACTION = "Jump";
        public const string STOLENACTION = "StolenAction";
        public const string STOLENACTIONDOWN = "StolenActionDown";
        public const string TOOLACTIONACTIONNAME = "ToolAction";
        public const string TOOLPICKUPDROPACTIONNAME = "ToolPickup/Drop";
        public const string MEDITATEACTIONNAME = "Meditate";
        public const string POSSESSACTION = "Possess";

        //To be visible, the object most be between 0 and 1 for both X and Y positions
        public static bool IsVisibleToCamera(Camera mainCamera, Vector3 objectPosition)
        {
            Vector3 cameraVision = mainCamera.WorldToViewportPoint(objectPosition);
            return (cameraVision.x >= 0 && cameraVision.y >= 0)
                    && (cameraVision.x <= 1 && cameraVision.y <= 1);
        }

        public static bool FastApproximately(float a, float b, float threshold)
        {
            return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
        }

        public static GameObject GetChildOfGivenType(GameObject parent, System.Type childType)
        {
            return parent.GetComponentInChildren(childType).gameObject;
        }

        public static PlayerInput InstantiatePlayerWithKeyboard(GameObject player,
            Vector2? playerTransformPosition)
        {
            string controlScheme = "";

            if (player.CompareTag(LEFTPLAYERTAG))
            {
                controlScheme = LEFTCONTROLSCHEME;
            }

            else if (player.CompareTag(RIGHTPLAYERTAG))
            {
                controlScheme = RIGHTCONTROLSCHEME;
            }

            PlayerInput playerComponentPlayerInput =  PlayerInput.Instantiate
            (
                player,
                controlScheme: controlScheme,
                pairWithDevice: GetKeyboard()
            );

            playerComponentPlayerInput.gameObject.transform.position =
                (Vector2) (!playerTransformPosition.HasValue 
                ? Vector2.zero : playerTransformPosition);

            return playerComponentPlayerInput;
        }

        public static PlayerInput PlayerManualCreation
        (
            string tag, 
            string controlScheme
        )
        {
            GameObject player = new GameObject(tag, typeof(PlayerInput))
            {
                tag = tag
            };

            PlayerInput input = PlayerInput.Instantiate
            (
                player,
                controlScheme: controlScheme,
                pairWithDevice: GetKeyboard()
            );

            Object.Destroy(player);

            return input;
        }

        public static GameObject PlayerManualInstantiation
        (
            PlayerInput input, 
            GameObject player, 
            string controlScheme, 
            Vector2 position,
            UnityAction<InputAction.CallbackContext>[] unityActions,
            string[] actionNames
        )
        {
            SetUpPlayerInput(input, actionNames, unityActions, controlScheme);

            player.transform.position = position;

            return player;
        }

        public static GameObject RightPlayerManualInstantiation(Vector2 position)
        {
            PlayerInput input = PlayerManualCreation(RIGHTPLAYERTAG, RIGHTCONTROLSCHEME);

            GameObject rightPlayer = input.gameObject;

            rightPlayer.AddComponent<GhostAnchor>();
            GhostMovement ghostMovement = rightPlayer.AddComponent<GhostMovement>();
            GhostPossession ghostPossesion = rightPlayer.AddComponent<GhostPossession>();

            UnityAction<InputAction.CallbackContext>[] unityActions =
            {
                    ghostPossesion.OnPossession,
                    ghostPossesion.OnMonsterJump,
                    ghostPossesion.OnStolenAction,
                    ghostPossesion.OnStolenActionDown,
                    ghostMovement.OnMove
            };

            string[] actionNames =
            {
                POSSESSACTION,
                JUMPACTION,
                STOLENACTION,
                STOLENACTIONDOWN,
                MOVEMENTACTION
            };

            return PlayerManualInstantiation(input, rightPlayer, RIGHTCONTROLSCHEME,
                position, unityActions, actionNames);
        }

        public static void SetUpPlayerInput
        (
            PlayerInput input,
            string[] actionNames,
            UnityAction<InputAction.CallbackContext>[] unityEvents,
            string controllerScheme
        )
        {
            if (!input.actions)
            {
                input.actions = Resources.Load<InputActionAsset>(CONTROLLERPREFAB);
            }
            input.defaultControlScheme = controllerScheme;
            input.defaultActionMap = PLAYERACTIONMAP;
            input.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
            if (input.currentActionMap == null)
            {
                input.currentActionMap = input.actions.FindActionMap(input.defaultActionMap);
            }

            if (input.currentControlScheme != controllerScheme)
            {
                InputDevice[] devices = new InputDevice[] { GetKeyboard() };
                input.SwitchCurrentControlScheme(controllerScheme, devices);
            }

            input.actionEvents
                = NamesListToEventList(input.currentActionMap, actionNames);

            AddListenerToActionEventList(input, unityEvents);
        }

        // TODO: Decide if we should keep this here or move it fully t GameManagerController Script
        public static IEnumerator SceneHandler(bool hasWon, float timeToWaitBeforeLoadingScene)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            yield return new WaitForSecondsRealtime(timeToWaitBeforeLoadingScene);

            if (hasWon)
            {
                if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {
                    Time.timeScale = 1;

                    SceneManager.LoadSceneAsync(sceneIndex + 1);
                }

                else
                {
                    Actions.OnFloorStateChange(true);
                }
            }

            else
            {
                SceneManager.LoadSceneAsync(sceneIndex);
            }
        }

        public static void ActionHandler
            (Actions.Names actionName, object parameter, object context)
        {
            // Catch exception that triggers in the tests
            try
            {
                if (actionName == Actions.Names.OnRequestAudio)
                {
                    Actions.OnRequestAudio((Clips)parameter);
                }

                else if (actionName == Actions.Names.OnRoomStateChange)
                {
                    Actions.OnRoomStateChange((bool)parameter);
                }
            }

            catch (System.NullReferenceException e)
            {
                Debug.LogWarning("Context: " + context + "\n Exception: " + e);
            }
        }

        private static Keyboard GetKeyboard()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard == null)
            {
                keyboard = InputSystem.AddDevice<Keyboard>();
            }

            return keyboard;
        }

        private static ReadOnlyArray<PlayerInput.ActionEvent> NamesListToEventList
        (
            InputActionMap playerActionMap,
            string[] actionNames
            
        )
        {
            PlayerInput.ActionEvent[] actionEvents
                = new PlayerInput.ActionEvent[actionNames.Length];

            for (int index = 0; index < actionNames.Length; index++)
            {
                InputAction action = playerActionMap.FindAction(actionNames[index]);
                PlayerInput.ActionEvent actionEvnt = new PlayerInput.ActionEvent(action);
                actionEvents[index] = actionEvnt;
            }

            return new ReadOnlyArray<PlayerInput.ActionEvent>(actionEvents);
        }

        private static void AddListenerToActionEventList
        (
            PlayerInput input, 
            UnityAction<InputAction.CallbackContext>[] unityEvents
        )
        {
            for (int index = 0; index < unityEvents.Length; index++)
            {
                input.actionEvents[index].AddListener(unityEvents[index]);
            }
        }
    }
}