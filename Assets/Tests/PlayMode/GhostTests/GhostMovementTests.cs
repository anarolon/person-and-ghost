using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Utils;
using UnityEngine.Events;
using PersonAndGhost.Ghost;

namespace PersonAndGhost.PlayMode.GhostTests
{
    public class GhostMovementTests : InputTestFixture
    {
        private Transform _ghostTransform;

        private void PlayModeSetUp()
        {
            string controlScheme = Utility.RIGHTCONTROLSCHEME;

            PlayerInput input = Utility.PlayerManualCreation(
                Utility.RIGHTPLAYERTAG, controlScheme);

            GameObject rightPlayer = input.gameObject;

            GhostMovement ghostMovement = rightPlayer.AddComponent<GhostMovement>();

            UnityAction<InputAction.CallbackContext>[] unityActions =
            {
                    ghostMovement.OnMove
            };

            string[] actionNames =
            {
                Utility.MOVEMENTACTION
            };

            _ghostTransform = Utility.PlayerManualInstantiation(input, rightPlayer,
                controlScheme, Vector2.zero, unityActions, actionNames).transform;
        }

        [UnityTearDown]
        public new IEnumerator TearDown()
        {
            Object.Destroy(_ghostTransform);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator MoveUp()
        {
            PlayModeSetUp();

            float previousPos = _ghostTransform.position.y;

            Press(Keyboard.current.upArrowKey);

            yield return new WaitForFixedUpdate();

            Assert.Greater(_ghostTransform.position.y, previousPos, "Moved upwards.");
        }

        [UnityTest]
        public IEnumerator MoveDown()
        {
            PlayModeSetUp();

            float previousPos = _ghostTransform.position.y;

            Press(Keyboard.current.downArrowKey);

            yield return new WaitForFixedUpdate();

            Assert.Less(_ghostTransform.position.y, previousPos, "Moved downwards.");
        }

        [UnityTest]
        public IEnumerator MoveRight()
        {
            PlayModeSetUp();

            float previousPos = _ghostTransform.position.x;

            Press(Keyboard.current.rightArrowKey);

            yield return new WaitForFixedUpdate();

            Assert.Greater(_ghostTransform.position.x, previousPos, "Moved to right.");
        }

        [UnityTest]
        public IEnumerator MoveLeft()
        {
            PlayModeSetUp();

            float previousPos = _ghostTransform.position.x;

            Press(Keyboard.current.leftArrowKey);

            yield return new WaitForFixedUpdate();

            Assert.Less(_ghostTransform.position.x, previousPos, "Moved to left.");
        }
    }
}


