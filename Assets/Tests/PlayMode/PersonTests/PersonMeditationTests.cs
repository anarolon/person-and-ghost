using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Ghost;
using PersonAndGhost.Utils;
using PersonAndGhost.Person;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace PersonAndGhost.PlayMode.PersonTests
{
    public class PersonMeditationTests : InputTestFixture
    {
        private Transform _groundTransform;
        private GameObject _personPrefab;
        private Transform _personTransform;
        private PersonMovement _person;
        private Rigidbody2D _personRigidbody;
        private Transform _ghostTransform;
        private GhostAnchor _anchor;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            GameObject ghost = new GameObject(Utility.RIGHTPLAYERTAG);
            _ghostTransform = ghost.transform;
            _anchor = ghost.AddComponent<GhostAnchor>();
            _ghostTransform.gameObject.SetActive(false);

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
            _personRigidbody = _person.GetComponent<Rigidbody2D>();

            yield return new WaitUntil(() => _person.IsOnGround);
        }

        [UnityTearDown]
        public new IEnumerator TearDown()
        {
            Object.Destroy(_groundTransform);
            Object.Destroy(_personTransform);
            Object.Destroy(_ghostTransform);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator StopWhenPersonIsMeditating()
        {
            yield return PlayModeSetUp();

            Press(Keyboard.current.aKey);

            yield return new WaitWhile(() => _personRigidbody.velocity == Vector2.zero);

            Assert.AreNotEqual(Vector2.zero, _personRigidbody.velocity);

            Release(Keyboard.current.aKey);
            Press(Keyboard.current.eKey);

            yield return new WaitUntil(() => 
                _personRigidbody.velocity == Vector2.zero);

            Assert.AreEqual(Vector2.zero, _personRigidbody.velocity);

            Release(Keyboard.current.eKey);
            Press(Keyboard.current.spaceKey);
            Press(Keyboard.current.dKey);

            yield return new WaitForFixedUpdate();

            Assert.AreEqual(Vector2.zero, _personRigidbody.velocity);

            Press(Keyboard.current.eKey);

            yield return new WaitWhile(() =>
                _personRigidbody.velocity == Vector2.zero);

            Assert.AreNotEqual(Vector2.zero, _personRigidbody.velocity);
        }
        
        [UnityTest]
        public IEnumerator IncreaseGhostRangeWhenPersonIsMeditating()
        {
            yield return PlayModeSetUp();

            //Line modified from 0.005 to 0.5 bug was found when running 
            //  it with the rest of the tests.
            float threshold = 0.5f;

            float offset = _anchor.AnchorRange * 1.5f;

            _ghostTransform.gameObject.SetActive(true);
            _ghostTransform.position = Vector2.one * offset;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            Assert.True(Utility.FastApproximately(_personTransform.position.x, 
                _ghostTransform.position.x, threshold), 
                "Ghost is anchored back to Person in x-axis.");

            Assert.True(Utility.FastApproximately(_personTransform.position.y, 
                _ghostTransform.position.y, threshold), 
                "Ghost is anchored back to Person in y-axis.");

            Press(Keyboard.current.eKey);

            yield return new WaitForFixedUpdate();

            _ghostTransform.position = Vector2.one * offset;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            Assert.False(Utility.FastApproximately(_personTransform.position.x,
                _ghostTransform.position.x, threshold),
                "Ghost is not anchored back to Person in x-axis.");

            Assert.False(Utility.FastApproximately(_personTransform.position.y,
                _ghostTransform.position.y, threshold),
                "Ghost is not anchored back to Person in y-axis.");

            Release(Keyboard.current.eKey);
            Press(Keyboard.current.eKey);

            yield return new WaitForFixedUpdate();

            Assert.True(Utility.FastApproximately(_personTransform.position.x,
                _ghostTransform.position.x, threshold),
                "Ghost is anchored back to Person in x-axis.");

            Assert.True(Utility.FastApproximately(_personTransform.position.y,
                _ghostTransform.position.y, threshold),
                "Ghost is anchored back to Person in y-axis.");
        }
    }
}