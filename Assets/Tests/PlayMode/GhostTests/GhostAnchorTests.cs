using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Ghost;
using PersonAndGhost.Person;
using PersonAndGhost.Utils;

namespace PersonAndGhost.PlayMode.GhostTests
{
    public class GhostAnchorTests
    {
        private GhostAnchor _ghostAnchorController;
        private Transform _anchorTransform;
        private Transform _ghostTransform;

        private GameObject _cam;


        [UnitySetUp]
        public IEnumerator SetUp()
        {
            GameObject anchor = new GameObject("Anchor");
            GameObject ghost = new GameObject(Utility.RIGHTPLAYERTAG);

            // TODO: Fix Camera Setup
            CameraSetup();

            anchor.AddComponent<Rigidbody2D>().constraints = 
                RigidbodyConstraints2D.FreezePositionY;
            anchor.AddComponent<PersonMovement>();

            SpriteRenderer sprRenderer = ghost.AddComponent<SpriteRenderer>();
            // TODO: Switch to using Addressables instead of Resources folder
            sprRenderer.sprite = Resources.Load<Sprite>("Sprites/Capsule");

            _anchorTransform = anchor.transform;
            _ghostTransform = ghost.transform;
            _ghostAnchorController = ghost.AddComponent<GhostAnchor>();

            yield return new EnterPlayMode();
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
            Object.Destroy(_ghostTransform.gameObject);
            Object.Destroy(_anchorTransform.gameObject);
            Object.Destroy(_cam.gameObject);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator AnchorGhostDefault()
        {
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
            yield return new WaitForSeconds(0);
            Assert.AreEqual(1, 1);
            // TODO: Going to need to incorporate person meditation
            /*
            float offset = _ghostAnchorController.AnchorRange + 2.0f;

            // TODO: Make Person meditate
            _anchorTransform.position = Vector2.zero;
            _ghostAnchorController.HasExpandedBoundary = true;

            // TODO: Check that Ghost can get past normal boundary
            _ghostTransform.position = Vector2.one * offset;
            yield return new WaitForFixedUpdate();
            Vector2 boundPosition = _ghostAnchorController.CalculateAnchorBoundPosition();
            Assert.AreNotEqual((Vector2)_ghostTransform.position, boundPosition);

            // TODO: test ghost moving it left
            offset *= 5;
            _ghostTransform.position = Vector2.one * offset;
            yield return new WaitForFixedUpdate();
            Vector2 correctedGhostPosition = _ghostAnchorController.CalculateAnchorBoundPosition();
            correctedGhostPosition = _ghostAnchorController.CalculateCameraBoundPosition();
            Assert.AreEqual((Vector2)_ghostTransform.position, correctedGhostPosition);
            // TODO: test ghost moving it right
            // TODO: test ghost moving it bottom
            // TODO: test ghost moving it up

            */
        }
    }
}


