using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Utils;
using PersonAndGhost.Tools;
using PersonAndGhost.Person;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.TestTools;

namespace PersonAndGhost.PlayMode.PersonTests
{
    public class PersonGrappleTests : InputTestFixture
    {
        private Transform _groundTransform;
        private Transform _wallTransform;
        private GameObject _personPrefab;
        private GameObject _grapplingHookPrefab;
        private Transform _personTransform;
        private PersonMovement _person;
        private GrapplingHook _grapplingHook;
        private Transform _grappleAimTransform;

        // Keys
        private KeyControl _pickupKey => Keyboard.current.qKey;
        private KeyControl _toolActionKey => Keyboard.current.rKey;
        private KeyControl _moveLeftKey => Keyboard.current.dKey;
        private KeyControl _jumpKey => Keyboard.current.spaceKey;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            GameObject groundPrefab
                = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            _groundTransform = Object.Instantiate(groundPrefab).transform;
            _groundTransform.localScale *= 2.5f;

            GameObject wallPrefab
                = Resources.Load<GameObject>(Utility.WALLPREFABPATH);
            _wallTransform = Object.Instantiate(wallPrefab).transform;
            _wallTransform.localScale *= 2.5f;

            _personPrefab = Resources.Load<GameObject>(Utility.LEFTPLAYERPREFAB);
            _grapplingHookPrefab = Resources.Load<GameObject>(Utility.GRAPPLINGHOOKPREFABPATH);

            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
            _personTransform = Utility.InstantiatePlayerWithKeyboard(
                _personPrefab, default).gameObject.transform;
            _person = _personTransform.GetComponent<PersonMovement>();
            _grapplingHook = Object.Instantiate(
                _grapplingHookPrefab).GetComponent<GrapplingHook>();

            yield return new WaitUntil(() => _person.IsOnGround);

            //Pickup
            _grapplingHook.transform.position = _personTransform.position;
            Press(_pickupKey);
            yield return new WaitForFixedUpdate();
            Release(_pickupKey);
            //Enter Grapple State
            Press(_toolActionKey);
            yield return new WaitUntil(() => _person.CurrentState.Contains(
            "GrappleAimState"));
            Release(_toolActionKey);
            _grappleAimTransform = _person.transform.Find("GrappleAim(Clone)");
            yield return new WaitForFixedUpdate();



        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.Destroy(_groundTransform);
            Object.Destroy(_wallTransform);
            Object.Destroy(_grappleAimTransform);
            Object.Destroy(_personTransform);
            Object.Destroy(_grapplingHook);
            

            yield return new ExitPlayMode();
        }



        [UnityTest]
        public IEnumerator CreateGrappleAim()
        {
            yield return PlayModeSetUp();
            //Press(_toolActionKey);
            //yield return new WaitForFixedUpdate();
            //Release(_toolActionKey);
            //yield return new WaitForFixedUpdate();

            Assert.AreEqual(_person.CurrentState,
                "PersonAndGhost.Person.States.GrappleAimState");
            Assert.AreNotEqual(_grappleAimTransform, null);
        }

        [UnityTest]
        public IEnumerator ExitGrappleState()
        {
            
            yield return PlayModeSetUp();
            Press(_moveLeftKey);
            yield return new WaitUntil(() =>
             !_person.CanGrapple);
            Release(_moveLeftKey);
            Press(_jumpKey);
            yield return new WaitWhile(() => _person.CurrentState.Contains(
            "GrappleAimState"));
            Release(_jumpKey);
            Assert.AreNotEqual(_person.CurrentState,
                "PersonAndGhost.Person.States.GrappleAimState");
        }

        [UnityTest]
        public IEnumerator GrappleToTarget()
        {
            yield return PlayModeSetUp();
            _grappleAimTransform = _person.transform.Find("GrappleAim(Clone)");
            float prevDistanceToWall = Vector3.Distance(
                _personTransform.position, _wallTransform.position);
            _grappleAimTransform.Rotate(new Vector3(0, 0, -70));
            Press(_jumpKey);
            yield return new WaitForSeconds(1);
            Release(_jumpKey);
            float newDistanceToWall = Vector3.Distance(
                _personTransform.position, _wallTransform.position);
            Assert.Less(newDistanceToWall, prevDistanceToWall);

            // TODO: Check that Person leaves GrappleAim State after reaching the target point

            // TODO: Make grapple aim towards ground diagonally downwards
            // TODO: Shoot and check that Person is closer to ground object
        }

        [UnityTest]
        public IEnumerator MoveGrappleAim()
        {
            yield return PlayModeSetUp();
            _grappleAimTransform = _person.transform.Find("GrappleAim(Clone)");
            Vector3 prevPersonPosition = _personTransform.position;
            Quaternion prevGrappleAimRotation = _grappleAimTransform.rotation;
            Press(_moveLeftKey);
            yield return new WaitForSeconds(1);
            Release(_moveLeftKey);
            Assert.True(Utility.FastApproximately(prevPersonPosition.x,
                    _personTransform.position.x, 0.05f));
            Assert.False(Utility.FastApproximately(prevGrappleAimRotation.z,
                _grappleAimTransform.rotation.z, 0.05f));

        }

        [UnityTest]
        public IEnumerator PickupGrapple()
        {
            yield return PlayModeSetUp();
            //_grapplingHook.transform.position = _personTransform.position;
            //Press(_pickupKey);
            //yield return new WaitForFixedUpdate();
            //Release(_pickupKey);
            Assert.True(_grapplingHook.IsPickedUp);

        }

    }


}
