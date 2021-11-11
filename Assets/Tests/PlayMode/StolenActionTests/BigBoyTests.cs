using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using PersonAndGhost.Utils;
using PersonAndGhost.Ghost;
using PersonAndGhost.Person;

namespace PersonAndGhost.PlayMode
{
    public class BigBoyTests : InputTestFixture
    {
        private GameObject _groundPrefab;
        private Transform _groundTransform;
        
        private GameObject _cratePrefab;
        private Transform _crateTransform;

        private GameObject _bigBoy;
        private Transform _bigBoyTransform;

        private GameObject _ghost;
        private Transform _ghostTransform;
        private GhostPossession _possession;

        private GameObject _anchor;
        private GameObject _cam;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _anchor = new GameObject("Anchor");
            _anchor.AddComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            _anchor.AddComponent<PersonMovement>();

            _groundPrefab = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            _cratePrefab = Resources.Load<GameObject>(Utility.CRATE_PREFAB_PATH);
            _bigBoy = Resources.Load<GameObject>(Utility.BIGBOYPREFABPATH);

            _groundTransform = Object.Instantiate(_groundPrefab).transform;

            _crateTransform = Object.Instantiate(_cratePrefab).transform;
            _crateTransform.position = new Vector3(1, 1, 0);

            _bigBoyTransform = Object.Instantiate(_bigBoy).transform;
            _bigBoyTransform.localScale = Vector3.one * 0.5f;
            _bigBoyTransform.position = _crateTransform.position + new Vector3(-1, 1, 0);

            CameraSetup();

            yield return new WaitForSeconds(1);

            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
            _ghostTransform = Utility.RightPlayerManualInstantiation(Vector2.zero).transform;

            _possession = _ghostTransform.GetComponent<GhostPossession>();
            
            yield return new WaitForFixedUpdate();
        }

        private void CameraSetup()
        {
            _cam = new GameObject("Main Camera");
            Camera _camCamera = _cam.AddComponent<Camera>();
            _cam.tag = "MainCamera";

            _camCamera.orthographic = true;
            _camCamera.orthographicSize = 4;

        }

        [UnityTearDown]
        public new IEnumerator TearDown()
        {
            Object.Destroy(_groundTransform);
            Object.Destroy(_ghostTransform);
            Object.Destroy(_bigBoyTransform);
            Object.Destroy(_crateTransform);
            Object.Destroy(_anchor);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator BigBoyPushPullCrateTest()
        {
            yield return PlayModeSetUp();

            Press(Keyboard.current.upArrowKey);
            yield return new WaitUntil(() => _possession.IsNearAMonster);
            Release(Keyboard.current.upArrowKey);

            Press(Keyboard.current.numpadEnterKey);
            Assert.True(_possession.IsPossessing);
            Release(Keyboard.current.numpadEnterKey);

            yield return new WaitForFixedUpdate();

            // Grab Crate //
            Press(Keyboard.current.rightArrowKey);
            Press(Keyboard.current.slashKey);
            yield return new WaitUntil(() => _bigBoyTransform.gameObject.GetComponent<AIBigBoy>().isGrabbing);
            Release(Keyboard.current.slashKey);

            // Push Test //
            float bigBoyPreviousPos = _bigBoyTransform.position.x;
            yield return new WaitForSeconds(1); 
            Release(Keyboard.current.rightArrowKey);
            Assert.Greater(_bigBoyTransform.position.x, bigBoyPreviousPos, "Big Boy pushed Crate.");

            // Pull Test //
            bigBoyPreviousPos = _bigBoyTransform.position.x;
            Press(Keyboard.current.leftArrowKey);
            yield return new WaitForSeconds(1);
            Release(Keyboard.current.leftArrowKey);
            Assert.Less(_bigBoyTransform.position.x, bigBoyPreviousPos, "Big Boy pulled Crate.");

            // Check that Crate was updated by Grab to be movable //
            Assert.True(_crateTransform.gameObject.GetComponent<Movable>().isBeingMoved);
            yield return new WaitForFixedUpdate();

            // Release Crate //
            Press(Keyboard.current.slashKey);
            yield return new WaitUntil(() => !_bigBoyTransform.gameObject.GetComponent<AIBigBoy>().isGrabbing);
            Release(Keyboard.current.slashKey);
            Assert.False(_crateTransform.gameObject.GetComponent<Movable>().isBeingMoved);
        }
    }
}