using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Utils;

namespace PersonAndGhost.PlayMode.GhostTests
{
    public class GhostMovementTests : InputTestFixture
    {
        private GameObject _ghostPrefab;
        private Transform _ghostTransform;
        private Keyboard _keyboard;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _ghostPrefab = Resources.Load<GameObject>(Utility.RIGHTPLAYERPREFABPATH);

            yield return new EnterPlayMode();
        }

        private void PlayModeSetUp()
        {
            PlayerInput ghostPlayerInput = Utility.InstantiatePlayerWithKeyboard(
                _ghostPrefab, default);
            var ghostDevices = ghostPlayerInput.devices;
            int keyboardIndex = ghostDevices.Count <= 1 ? 0 :
                ghostDevices.IndexOf(device => device.GetType() == typeof(Keyboard));

            _ghostTransform = ghostPlayerInput.transform;
            _keyboard = (Keyboard)ghostPlayerInput.devices[keyboardIndex];
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

            Press(_keyboard.upArrowKey);

            yield return new WaitForFixedUpdate();

            Assert.Greater(_ghostTransform.position.y, previousPos, "Moved upwards.");
        }

        [UnityTest]
        public IEnumerator MoveDown()
        {
            PlayModeSetUp();

            float previousPos = _ghostTransform.position.y;

            Press(_keyboard.downArrowKey);

            yield return new WaitForFixedUpdate();

            Assert.Less(_ghostTransform.position.y, previousPos, "Moved downwards.");
        }

        [UnityTest]
        public IEnumerator MoveRight()
        {
            PlayModeSetUp();

            float previousPos = _ghostTransform.position.x;

            Press(_keyboard.rightArrowKey);

            yield return new WaitForFixedUpdate();

            Assert.Greater(_ghostTransform.position.x, previousPos, "Moved to right.");
        }

        [UnityTest]
        public IEnumerator MoveLeft()
        {
            PlayModeSetUp();

            float previousPos = _ghostTransform.position.x;

            Press(_keyboard.leftArrowKey);
            
            yield return new WaitForFixedUpdate();

            Assert.Less(_ghostTransform.position.x, previousPos, "Moved to left.");
        }
    }
}


