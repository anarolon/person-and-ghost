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
    public class BirdTests : InputTestFixture
    {
        private GameObject _groundPrefab;
        private Transform _groundTransform;
        private GameObject _bird;
        private AIBird _birdScript;
        private Transform _birdTransform;
        private GameObject _ghost;
        private Transform _ghostTransform;
        private GhostPossession _possession;
        private GameObject _personPrefab;
        private Transform _personTransform;
        private Rigidbody2D _personRigidbody;
        private PersonMovement _person;
        private GhostAnchor _anchor;
        private GameObject _cam;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _ghost = Resources.Load<GameObject>(Utility.RIGHTPLAYERPREFABPATH);
            _ghostTransform = _ghost.transform;
            _anchor = _ghost.AddComponent<GhostAnchor>();
        
            _groundPrefab = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            _groundTransform = Object.Instantiate(_groundPrefab).transform;
    
            _personPrefab = Resources.Load<GameObject>(Utility.LEFTPLAYERPREFAB);
            _bird = Resources.Load<GameObject>(Utility.CLAWEDBIRDPREFABPATH);

            _birdTransform = Object.Instantiate(_bird).transform;
            _birdTransform.position = _groundTransform.position + new Vector3(4, 3, 0);
            _birdScript = _birdTransform.gameObject.GetComponent<AIBird>();

            CameraSetup();

            yield return new WaitForSeconds(1);
            
            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
            _personTransform = Utility.InstantiatePlayerWithKeyboard(
                _personPrefab, default).gameObject.transform;
            _person = _personTransform.GetComponent<PersonMovement>();
            _personRigidbody = _person.GetComponent<Rigidbody2D>();

            yield return new WaitUntil(() => _person.IsOnGround);

            _ghostTransform =
                Utility.RightPlayerManualInstantiation(Vector2.zero).transform;

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
            Object.Destroy(_personTransform);
            Object.Destroy(_ghostTransform);
            Object.Destroy(_birdTransform);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator BirdPickUpAndDropTest()
        {
            yield return PlayModeSetUp();

            Press(Keyboard.current.upArrowKey);
            yield return new WaitUntil(() => _possession.IsNearAMonster);
            Release(Keyboard.current.upArrowKey);

            // Possess Bird //
            Press(Keyboard.current.numpadEnterKey);
            Assert.True(_possession.IsPossessing);
            Release(Keyboard.current.numpadEnterKey);

            yield return new WaitForFixedUpdate();
 
            // Place Bird above Person //
            _birdTransform.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _birdTransform.position = _personTransform.position + new Vector3(0, .85f, 0);
            yield return new WaitUntil(() => _birdScript.claws.isPersonNearby);

            // Pick Up Person //
            Press(Keyboard.current.slashKey);
            yield return new WaitForFixedUpdate();

            // Check that Person is being carried //
            Assert.True(_person.IsBeingCarried);
            yield return new WaitForFixedUpdate();

            // Drop Person // 
            Release(Keyboard.current.slashKey);
            Press(Keyboard.current.slashKey);
            yield return new WaitForFixedUpdate();
            Assert.False(_person.IsBeingCarried);
        }

    }
}