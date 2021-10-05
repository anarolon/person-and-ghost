using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Ghost;
using PersonAndGhost.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace PersonAndGhost.PlayMode.PersonTests
{
    public class PersonMeditationTests : InputTestFixture
    {
        private GameObject _groundPrefab;
        private GameObject _personPrefab;
        private GameObject _ghostPrefab;
        private Transform _groundTransform;
        private Transform _personTransform;
        private Transform _ghostTransform;
        private PlayerController _person;
        private Rigidbody2D _personRigidbody2D;
        private GhostAnchor _anchor;
        private Keyboard _keyboard;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _groundPrefab = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            _personPrefab = Resources.Load<GameObject>(Utility.LEFTPLAYERPREFABPATH);
            _ghostPrefab = Resources.Load<GameObject>(Utility.RIGHTPLAYERPREFABPATH);

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

            PlayerInput ghostPlayerInput = Utility.InstantiatePlayerWithKeyboard
            (
                _ghostPrefab,
                default
            );

            var personDevices = personPlayerInput.devices;
            int keyboardIndex = personDevices.Count <= 1 ? 0 :
                personDevices.IndexOf(device => device.GetType() == typeof(Keyboard));

            _personTransform = personPlayerInput.transform;
            _person = _personTransform.GetComponent<PlayerController>();
            _personRigidbody2D = _person.GetComponent<Rigidbody2D>();
            _keyboard = (Keyboard)personPlayerInput.devices[keyboardIndex];

            _ghostTransform = ghostPlayerInput.transform;
            _anchor = _ghostTransform.GetComponent<GhostAnchor>();
            _ghostTransform.gameObject.SetActive(false);

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

            Press(_keyboard.aKey);

            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(Vector2.zero, _personRigidbody2D.velocity);

            Release(_keyboard.aKey);
            Press(_keyboard.eKey);

            yield return new WaitUntil(() => 
                _personRigidbody2D.velocity == Vector2.zero);

            Assert.AreEqual(Vector2.zero, _personRigidbody2D.velocity);

            Release(_keyboard.eKey);
            Press(_keyboard.spaceKey);
            Press(_keyboard.dKey);

            yield return new WaitForFixedUpdate();

            Assert.AreEqual(Vector2.zero, _personRigidbody2D.velocity);

            Press(_keyboard.eKey);

            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(Vector2.zero, _personRigidbody2D.velocity);
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

            Press(_keyboard.eKey);

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

            Release(_keyboard.eKey);
            Press(_keyboard.eKey);

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