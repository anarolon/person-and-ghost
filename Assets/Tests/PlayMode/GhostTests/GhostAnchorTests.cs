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
        private GhostAnchor _anchor;
        private Transform _anchorTransform;
        private Transform _ghostTransform;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            GameObject anchor = new GameObject("Anchor");
            GameObject ghost = new GameObject(Utility.RIGHTPLAYERTAG);

            anchor.AddComponent<Rigidbody2D>().constraints = 
                RigidbodyConstraints2D.FreezePositionY;
            anchor.AddComponent<PersonMovement>();

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
            Vector2 uncorrectedGhostPosition = _ghostTransform.position;
            float offset = _anchor.AnchorRange + 5.0f;

            _anchorTransform.position = Vector2.one * offset;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            // TODO: Make this a method either here or in ghostAnchor Script for easier resueability
            Vector2 correctedGhostPosition = (Vector2)_anchorTransform.position +
                (uncorrectedGhostPosition - (Vector2)_anchorTransform.position) *
                (_anchor.AnchorRange /
                Vector3.Distance(uncorrectedGhostPosition, _anchorTransform.position)
                );

            Assert.AreEqual((Vector2)_ghostTransform.position, correctedGhostPosition);

            _ghostTransform.position = Vector2.one * -offset;
            uncorrectedGhostPosition = _ghostTransform.position;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            correctedGhostPosition = (Vector2)_anchorTransform.position +
                (uncorrectedGhostPosition - (Vector2)_anchorTransform.position) *
                (_anchor.AnchorRange /
                Vector3.Distance(uncorrectedGhostPosition, _anchorTransform.position)
                );
            Assert.AreEqual((Vector2)_ghostTransform.position, correctedGhostPosition);
        }
    }
}


