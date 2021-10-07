using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using PersonAndGhost.Utils;
using PersonAndGhost.Person;
using PersonAndGhost.Tools;

namespace PersonAndGhost.PlayMode.PersonTests
{
    public class PersonTests : InputTestFixture
    {
        private Transform _groundTransform;
        private GameObject _personPrefab;
        private Transform _personTransform;
        private PersonMovement _person;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            GameObject groundPrefab 
                = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            _groundTransform = Object.Instantiate(groundPrefab).transform;
            _groundTransform.localScale *= 2.5f;

            _personPrefab = Resources.Load<GameObject>(Utility.LEFTPLAYERPREFAB);

            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
            _personTransform = Utility.InstantiatePlayerWithKeyboard(
                _personPrefab, default).gameObject.transform;

            _person = _personTransform.GetComponent<PersonMovement>();

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

            Press(Keyboard.current.aKey);

            yield return new WaitUntil(() => _personTransform.position.x < previousPos);

            Assert.Less(_personTransform.position.x, previousPos, "Moved to the left.");
        }

        [UnityTest]
        public IEnumerator MoveRight()
        {
            yield return PlayModeSetUp();

            float previousPos = _personTransform.position.x;

            Press(Keyboard.current.dKey);

            yield return new WaitUntil(() => _personTransform.position.x > previousPos);

            Assert.Greater(_personTransform.position.x, previousPos, "Moved to right.");
        }

        [UnityTest]
        public IEnumerator Jump()
        {
            yield return PlayModeSetUp();

            Press(Keyboard.current.spaceKey);

            yield return new WaitUntil(() => !_person.IsOnGround);

            Assert.False(_person.IsOnGround);

            yield return new WaitUntil(() => _person.IsOnGround);

            Assert.True(_person.IsOnGround);
        }

        [UnityTest]
        public IEnumerator ToolPickUpAndDrop()
        {
            yield return PlayModeSetUp();

            GameObject toolPrefab = Resources.Load<GameObject>(
                Utility.CLIMBINGGAUNTLETPREFABPATH);
            GameObject tool = Object.Instantiate(
                toolPrefab, Vector3.zero, Quaternion.identity);

            yield return new WaitForFixedUpdate();

            ClimbingGauntlet toolScript = tool.GetComponent<ClimbingGauntlet>();

            Assert.False(toolScript.IsPickedUp);

            Press(Keyboard.current.qKey);

            yield return new WaitUntil(() => toolScript.IsPickedUp);

            Assert.True(toolScript.IsPickedUp);

            Release(Keyboard.current.qKey);
            Press(Keyboard.current.zKey);

            yield return new WaitWhile(() => toolScript.IsPickedUp);

            Assert.False(toolScript.IsPickedUp);

            Object.Destroy(tool);
        }

        [UnityTest]
        public IEnumerator ToolActionClimbingGauntlet()
        {
            yield return PlayModeSetUp();

            GameObject toolPrefab = Resources.Load<GameObject>(
                Utility.CLIMBINGGAUNTLETPREFABPATH);
            GameObject tool 
                = Object.Instantiate(toolPrefab, Vector3.zero, Quaternion.identity);

            ClimbingGauntlet toolScript = tool.GetComponent<ClimbingGauntlet>();

            GameObject wallPrefab = Resources.Load<GameObject>(
                Utility.WALLPREFABPATH);
            GameObject wall = Object.Instantiate(
                wallPrefab, new Vector3(0.77f, 3, 0), Quaternion.identity);

            yield return new WaitForFixedUpdate();

            Press(Keyboard.current.qKey);

            yield return new WaitUntil(() => toolScript.IsPickedUp);

            Release(Keyboard.current.qKey);
            Press(Keyboard.current.spaceKey);

            yield return new WaitUntil(() => !_person.IsOnGround);

            Release(Keyboard.current.spaceKey);
            Press(Keyboard.current.rKey);

            yield return new WaitForFixedUpdate();

            Assert.False(_person.IsOnGround);

            Release(Keyboard.current.rKey);
            Press(Keyboard.current.spaceKey);

            yield return new WaitUntil(() => _person.IsOnGround);

            Assert.True(_person.IsOnGround);

            Object.Destroy(tool);
            Object.Destroy(wall);
        }
    }
}