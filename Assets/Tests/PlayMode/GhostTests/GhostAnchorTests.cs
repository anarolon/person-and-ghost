using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Ghost;
using PersonAndGhost.Utils;

namespace PersonAndGhost.PlayMode.GhostTests
{
    public class GhostAnchorTests: InputTestFixture
    {
        
        private GhostAnchor _ghostAnchorController;
        private Transform _anchorTransform;
        private Transform _ghostTransform;

        private GameObject _personPrefab;
        private GameObject _cam;

        // Keys
        private KeyControl _meditationKey => Keyboard.current.eKey;


        [UnitySetUp]
        public IEnumerator SetUp()
        {
            GameObject groundPrefab
                = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            Transform groundTransform = Object.Instantiate(groundPrefab).transform;
            groundTransform.localScale *= 2.5f;

            //GameObject person = new GameObject("Anchor");
            _personPrefab = Resources.Load<GameObject>(Utility.LEFTPLAYERPREFAB);

            // TODO: Fix Camera Setup
            CameraSetup();
            

            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
           
            _anchorTransform = Utility.InstantiatePlayerWithKeyboard(
                _personPrefab, default).gameObject.transform;
            GameObject ghost = new GameObject(Utility.RIGHTPLAYERTAG);



            //person.AddComponent<Rigidbody2D>().constraints = 
            //    RigidbodyConstraints2D.FreezePositionY;
            //person.AddComponent<PersonMovement>();


            SpriteRenderer sprRenderer = ghost.AddComponent<SpriteRenderer>();
            // TODO: Switch to using Addressables instead of Resources folder
            sprRenderer.sprite = Resources.Load<Sprite>("Sprites/Capsule");

            //_anchorTransform = person.transform;
            _ghostTransform = ghost.transform;
            _ghostAnchorController = ghost.AddComponent<GhostAnchor>();

            yield return new WaitForSeconds(1);

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
        public IEnumerator TearDown()
        {
            //Object.Destroy(_ghostTransform.gameObject);
            //Object.Destroy(_anchorTransform.gameObject);
            Object.Destroy(_cam.gameObject);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator AnchorGhostDefault()
        {
            yield return PlayModeSetUp();
            float offset = _ghostAnchorController.AnchorRange + 5.0f;

            _anchorTransform.position = Vector2.one * offset;

            yield return new WaitForFixedUpdate();

            Vector2 correctedGhostPosition = _ghostAnchorController.CalculateAnchorBoundPosition();
            correctedGhostPosition = _ghostAnchorController.CalculateCameraBoundPosition();
            Assert.AreEqual((Vector2)_ghostTransform.position, correctedGhostPosition);

            
            _ghostTransform.position = Vector2.one * -offset;

            yield return new WaitForFixedUpdate();


            correctedGhostPosition = _ghostAnchorController.CalculateAnchorBoundPosition();
            correctedGhostPosition = _ghostAnchorController.CalculateCameraBoundPosition();
            Assert.AreEqual((Vector2)_ghostTransform.position, correctedGhostPosition);
            
        }

        [UnityTest]
        public IEnumerator AnchorGhostMeditating()
        {
            yield return PlayModeSetUp();

            float offset = _ghostAnchorController.AnchorRange + 2.0f;
            Vector3 originalPos = _ghostTransform.position;

            Press(_meditationKey);
            yield return new WaitForFixedUpdate();
            Release(_meditationKey);
            Assert.AreEqual(1, 1);

            //Check that Ghost can get past normal boundary
            _ghostTransform.position = Vector2.one * offset;
            yield return new WaitForFixedUpdate();
            Vector2 boundPosition = _ghostAnchorController.CalculateAnchorBoundPosition();
            Debug.Log("normal bound pos: " + boundPosition + "; current position: " + (Vector2)_ghostTransform.position);
            Assert.AreNotEqual((Vector2)_ghostTransform.position, boundPosition);

            // right and up
            _ghostTransform.position = originalPos;
            offset *= 5;
            _ghostTransform.position = Vector2.one * offset;
            yield return new WaitForFixedUpdate();
            Vector2 correctedGhostPosition = _ghostAnchorController.CalculateCameraBoundPosition();
            Assert.AreEqual((Vector2)_ghostTransform.position, correctedGhostPosition);

            // left and down
            _ghostTransform.position = originalPos;
            offset *= 5;
            _ghostTransform.position = Vector2.one * -offset;
            yield return new WaitForFixedUpdate();
            correctedGhostPosition = _ghostAnchorController.CalculateCameraBoundPosition();
            Assert.AreEqual((Vector2)_ghostTransform.position, correctedGhostPosition);
        }
    }
}


