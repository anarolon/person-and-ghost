using UnityEngine;
using UnityEngine.InputSystem;

namespace PersonAndGhost.Utils
{
    public static class Utility
    {
        public const string MAINCAMERATAG = "MainCamera";
        public const string MONSTERTAG = "Monster";
        public const string LEFTPLAYERTAG = "Person";
        public const string RIGHTPLAYERTAG = "Ghost";
        public const string LEFTCONTROLSCHEME = "KeyboardLeft";
        public const string RIGHTCONTROLSCHEME = "KeyboardRight";
        public const string LEFTPLAYERPREFABPATH = "Prefabs/Person";
        public const string RIGHTPLAYERPREFABPATH = "Prefabs/Ghost";
        public const string BIRDPREFABPATH = "Prefabs/Bird";
        public const string GROUNDPREFABPATH = "Prefabs/Ground";
        public const string WALLPREFABPATH = "Prefabs/Wall";
        public const string CLIMBINGGAUNTLETPREFABPATH = "Prefabs/Climbing Gauntlet";

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

            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

            PlayerInput playerComponentPlayerInput =  PlayerInput.Instantiate
            (
                player,
                controlScheme: controlScheme,
                pairWithDevice: keyboard
            );

            playerComponentPlayerInput.gameObject.transform.position =
                (Vector2) (!playerTransformPosition.HasValue 
                ? Vector2.zero : playerTransformPosition);

            return playerComponentPlayerInput;
        }
    }
}