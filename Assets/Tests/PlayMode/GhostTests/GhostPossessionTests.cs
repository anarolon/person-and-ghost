using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.TestTools;
using PersonAndGhost.Utils;
using PersonAndGhost.Ghost;
using PersonAndGhost.Person;

namespace PersonAndGhost.PlayMode.GhostTests
{
    public class GhostPossessionTests : InputTestFixture
    {
        private Transform _ghostTransform;
        private GhostPossession _possession;
        private GameObject _anchor;
        private Transform _monsterTransform;
        private AIAgent _monster;

        private GameObject _cam;

        private const string BIRDNAME = "Bird (Clawed)(Clone)";

        // Keys
        private KeyControl _possessKey => Keyboard.current.numpadEnterKey;
        private KeyControl _moveUpKey => Keyboard.current.upArrowKey;
        private KeyControl _moveDownKey => Keyboard.current.downArrowKey;
        private KeyControl _moveLeftKey => Keyboard.current.leftArrowKey;
        private KeyControl _moveRightKey => Keyboard.current.rightArrowKey;


        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _anchor = new GameObject("Anchor");
            _anchor.AddComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.FreezePositionY;
            _anchor.AddComponent<PersonMovement>();

            // TODO: FIx Camera Setup
            CameraSetup();

            GameObject monstePrefab = Resources.Load<GameObject>(Utility.CLAWEDBIRDPREFABPATH);
            _monster = Object.Instantiate(monstePrefab).GetComponent<AIAgent>();
            _monsterTransform = _monster.transform;

            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
            _ghostTransform =
                Utility.RightPlayerManualInstantiation(Vector2.zero).transform;

            _possession = _ghostTransform.GetComponent<GhostPossession>();


            yield return new WaitUntil(() => _possession.IsNearAMonster);
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
            Object.Destroy(_ghostTransform);
            Object.Destroy(_anchor);
            Object.Destroy(_monsterTransform);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator GhostNearMonster()
        {
            yield return PlayModeSetUp();

            Assert.True(_possession.IsNearAMonster);

            yield return new WaitUntil(() => !_possession.IsNearAMonster);

            Assert.False(_possession.IsNearAMonster);
        }

        [UnityTest]
        public IEnumerator GhostPossessingMonster()
        {
            yield return PlayModeSetUp();

            Assert.False(_possession.IsPossessing);
            Assert.AreNotEqual(AIStateId.Possessed, _monster.stateMachine.currentState);

            Press(_possessKey);

            yield return new WaitForFixedUpdate();

            Assert.True(_possession.IsPossessing);
            Assert.AreEqual(AIStateId.Possessed, _monster.stateMachine.currentState);

            Release(_possessKey);
            Press(_possessKey);

            yield return new WaitForFixedUpdate();

            Assert.False(_possession.IsPossessing);
            Assert.AreNotEqual(AIStateId.Possessed, _monster.stateMachine.currentState);

            yield return new WaitUntil(() => !_possession.IsNearAMonster);

            Assert.False(_possession.IsNearAMonster);
        }

        [UnityTest]
        public IEnumerator MoveMonsterPossessedRight()
        {
            yield return PlayModeSetUp();

            Press(_possessKey);

            yield return new WaitForFixedUpdate();

            Release(_possessKey);

            float monsterPreviousPos = _monsterTransform.position.x;

            Press(_moveRightKey);

            yield return new WaitForFixedUpdate();

            //Line not needed to run test individually but 
            //  bug was found when running it with the rest of the tests.
            yield return new WaitForSeconds(1); 

            Assert.Greater(_monsterTransform.position.x, monsterPreviousPos, 
                "Monster Moved to the right.");
            Assert.True(Utility.FastApproximately(_ghostTransform.position.x, 
                _monsterTransform.position.x, 0.05f), "Ghost is following Monster.");
        }

        [UnityTest]
        public IEnumerator MoveMonsterPossessedLeft()
        {
            yield return PlayModeSetUp();

            Press(_possessKey);

            yield return new WaitForFixedUpdate();

            Release(_possessKey);

            float monsterPreviousPos = _monsterTransform.position.x;

            Press(_moveLeftKey);

            yield return new WaitForFixedUpdate();

            Assert.Less(_monsterTransform.position.x, monsterPreviousPos, 
                "Monster Moved to the left.");
            Assert.True(Utility.FastApproximately(_ghostTransform.position.x, 
                _monsterTransform.position.x, 0.05f), "Ghost is following Monster.");
        }

        [UnityTest]
        public IEnumerator MoveFlyingMonsterPossessedUp()
        {
            yield return PlayModeSetUp();

            //Identify flying monster
            Assert.AreEqual(BIRDNAME, _monsterTransform.gameObject.name);

            Press(_possessKey);

            yield return new WaitForFixedUpdate();

            Release(_possessKey);

            float monsterPreviousPos = _monsterTransform.position.y;

            Press(_moveUpKey);

            yield return new WaitForFixedUpdate();

            Assert.Greater(_monsterTransform.position.y, monsterPreviousPos, 
                "Monster Moved to the up.");
            Assert.True(Utility.FastApproximately(_ghostTransform.position.y, 
                _monsterTransform.position.y, 0.05f), "Ghost is following Monster.");
        }

        [UnityTest]
        public IEnumerator MoveFlyingMonsterPossessedDown()
        {
            yield return PlayModeSetUp();

            //Identify flying monster
            Assert.AreEqual(BIRDNAME, _monsterTransform.gameObject.name);

            Press(_possessKey);

            yield return new WaitForFixedUpdate();

            Release(_possessKey);

            float monsterPreviousPos = _monsterTransform.position.y;

            Press(_moveDownKey);

            yield return new WaitForFixedUpdate();

            Assert.Less(_monsterTransform.position.y, monsterPreviousPos, 
                "Monster Moved to the down.");
            Assert.True(Utility.FastApproximately(_ghostTransform.position.y, 
                _monsterTransform.position.y, 0.05f), "Ghost is following Monster.");
        }

        [UnityTest]
        public IEnumerator SpiritBarTest()
        {
            //This test will cause a bunch of logs notifying that there is no spirit bar slider in the scene
            yield return PlayModeSetUp();

            Assert.False(_possession.IsPossessing);
            Assert.AreNotEqual(AIStateId.Possessed, _monster.stateMachine.currentState);
            float initialSpiritEnergy = _possession.SpiritEnergy;

            Press(_possessKey);

            yield return new WaitForSeconds(0.5f);

            Assert.True(_possession.IsPossessing);
            Assert.AreEqual(AIStateId.Possessed, _monster.stateMachine.currentState);
            float drainedSpiritEnergy = _possession.SpiritEnergy;
            Assert.Less(drainedSpiritEnergy, initialSpiritEnergy);
            

            Release(_possessKey);
            Press(_possessKey);

            yield return new WaitForSeconds(0.5f);

            Assert.False(_possession.IsPossessing);
            Assert.AreNotEqual(AIStateId.Possessed, _monster.stateMachine.currentState);
            Assert.Greater(_possession.SpiritEnergy, drainedSpiritEnergy);

            yield return new WaitUntil(() => !_possession.IsNearAMonster);

            Assert.False(_possession.IsNearAMonster);
        }
    }
}