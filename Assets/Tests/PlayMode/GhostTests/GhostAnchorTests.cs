using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Ghost;

namespace PersonAndGhost.PlayMode.GhostTests
{
    public class GhostAnchorTests
    {
        private GhostAnchor _anchor;
        private Transform _anchorTransform;
        private Transform _ghostTransform;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            GameObject anchor = new GameObject();
            GameObject ghost = new GameObject();

            anchor.AddComponent<Rigidbody2D>().constraints = 
                RigidbodyConstraints2D.FreezePositionY;
            anchor.AddComponent<PlayerController>();

            _anchorTransform = anchor.transform;
            _ghostTransform = ghost.transform;
            _anchor = ghost.AddComponent<GhostAnchor>();

            yield return new EnterPlayMode();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.Destroy(_ghostTransform.gameObject);
            Object.Destroy(_anchorTransform.gameObject);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator AnchorGhost()
        {
            Vector2 previousPosition = _ghostTransform.position;
            float offset = _anchor.AnchorRange + 5.0f;

            _anchorTransform.position = Vector2.one * offset;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(_ghostTransform.position, _anchorTransform.position,
                "Ghost is anchored back to Anchor.");
            Assert.AreNotEqual(_ghostTransform.position, previousPosition,
                "Ghost exceeded the anchor range.");

            _ghostTransform.position = Vector2.one * offset;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(_ghostTransform.position, _anchorTransform.position,
                "Ghost is anchored back to Anchor.");
        }
    }
}


