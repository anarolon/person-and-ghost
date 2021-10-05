using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using PersonAndGhost.Utils;

namespace PersonAndGhost.PlayMode.PersonTests
{
    public class PersonTests : InputTestFixture
    {
        private GameObject _groundPrefab;
        private GameObject _personPrefab;
        private Transform _groundTransform;
        private Transform _personTransform;
        private PlayerController _person;
        private Keyboard _keyboard;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _groundPrefab = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            _personPrefab = Resources.Load<GameObject>(Utility.LEFTPLAYERPREFABPATH);
            _groundTransform = Object.Instantiate(_groundPrefab).transform;
            _groundTransform.localScale *= 2.5f;

            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
            PlayerInput personPlayerInput = Utility.InstantiatePlayerWithKeyboard
            (
                _personPrefab, 
                default
            );
            var personDevices = personPlayerInput.devices;
            int keyboardIndex = personDevices.Count <= 1 ? 0 :
                personDevices.IndexOf(device => device.GetType() == typeof(Keyboard));

            _personTransform = personPlayerInput.transform; 
            _person = _personTransform.GetComponent<PlayerController>();
            _keyboard = (Keyboard)personPlayerInput.devices[keyboardIndex];

            yield return new WaitUntil(() => _person.IsOnGround);
        }

        [UnityTearDown]
        public new IEnumerator TearDown()
        {
            Object.Destroy(_groundTransform);
            Object.Destroy(_personTransform);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator MoveLeft()
        {
            yield return PlayModeSetUp();

            float previousPos = _personTransform.position.x;

            Press(_keyboard.aKey);

            yield return new WaitForFixedUpdate();

            Assert.Less(_personTransform.position.x, previousPos, "Moved to the left.");
        }

        [UnityTest]
        public IEnumerator MoveRight()
        {
            yield return PlayModeSetUp();

            float previousPos = _personTransform.position.x;

            Press(_keyboard.dKey);

            yield return new WaitForFixedUpdate();

            Assert.Greater(_personTransform.position.x, previousPos, "Moved to the left.");
        }

        [UnityTest]
        public IEnumerator Jump()
        {
            yield return PlayModeSetUp();

            Press(_keyboard.spaceKey);

            yield return new WaitUntil(() => !_person.IsOnGround);

            Assert.False(_person.IsOnGround);

            yield return new WaitUntil(() => _person.IsOnGround);

            Assert.True(_person.IsOnGround);
        }

        [UnityTest]
        public IEnumerator ToolPickUpAndDrop()
        {
            yield return PlayModeSetUp();

            GameObject toolPrefab = Resources.Load<GameObject>(Utility.CLIMBINGGAUNTLETPREFABPATH);
            GameObject tool = Object.Instantiate(toolPrefab, Vector3.zero, Quaternion.identity);

            yield return new WaitForFixedUpdate();

            PlayerToolController personToolController = _person.GetComponent<PlayerToolController>();
            ClimbingGauntlet toolScript = tool.GetComponent<ClimbingGauntlet>();

            Assert.False(toolScript.OnPlayer);

            personToolController.InteractWithTool(toolScript);

            yield return new WaitForFixedUpdate();

            Assert.True(toolScript.OnPlayer);

            personToolController.InteractWithTool(toolScript);

            yield return new WaitForFixedUpdate();

            Assert.False(toolScript.OnPlayer);

            Object.Destroy(tool);
        }

        [UnityTest]
        public IEnumerator ToolActionClimbingGauntlet()
        {
            yield return PlayModeSetUp();

            GameObject toolPrefab = Resources.Load<GameObject>(Utility.CLIMBINGGAUNTLETPREFABPATH);
            GameObject tool = Object.Instantiate(toolPrefab, Vector3.zero, Quaternion.identity);

            GameObject wallPrefab = Resources.Load<GameObject>(Utility.WALLPREFABPATH);
            GameObject wall = Object.Instantiate(wallPrefab, new Vector3(1, 3, 0), Quaternion.identity);

            yield return new WaitForFixedUpdate();

            PlayerToolController personToolController = _person.GetComponent<PlayerToolController>();
            ClimbingGauntlet toolScript = tool.GetComponent<ClimbingGauntlet>();

            personToolController.InteractWithTool(toolScript);
            toolScript.Action();

            yield return new WaitUntil(() => !_person.IsOnGround);
            Assert.False(_person.IsOnGround);

            yield return new WaitUntil(() => _person.IsOnGround);
            Assert.True(_person.IsOnGround);

            Object.Destroy(tool);
            Object.Destroy(wall);
        }
    }
}