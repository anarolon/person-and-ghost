using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using PersonAndGhost.Utils;
using PersonAndGhost.Ghost;

namespace PersonAndGhost.PlayMode.GhostTests
{
    public class GhostPossessionTests : InputTestFixture
    {
        private GameObject _ghostPrefab;
        private GameObject _monsterPrefab;
        private Transform _ghostTransform;
        private Transform _monsterTransform;
        private GameObject _anchor;
        private GhostPossession _possession;
        private AIAgent _monster;
        private Keyboard _keyboard;

        private const string BIRDNAME = "Bird(Clone)";

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _anchor = new GameObject();
            _anchor.AddComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.FreezePositionY;
            _anchor.AddComponent<PlayerController>();

            _ghostPrefab = Resources.Load<GameObject>(Utility.RIGHTPLAYERPREFABPATH);
            _monsterPrefab = Resources.Load<GameObject>(Utility.BIRDPREFABPATH);

            _monster = Object.Instantiate(_monsterPrefab).GetComponent<AIAgent>();
            _monsterTransform = _monster.transform;

            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
            PlayerInput ghostPlayerInput = Utility.InstantiatePlayerWithKeyboard(
                _ghostPrefab, default);
            var ghostDevices = ghostPlayerInput.devices;
            int keyboardIndex = ghostDevices.Count <= 1 ? 0 :
                ghostDevices.IndexOf(device => device.GetType() == typeof(Keyboard));

            _ghostTransform = ghostPlayerInput.transform;
            _possession = ghostPlayerInput.GetComponent<GhostPossession>();
            _keyboard = (Keyboard)ghostPlayerInput.devices[keyboardIndex];

            yield return new WaitUntil(() => _possession.IsNearAMonster);
        }

        [UnityTearDown]
        public new IEnumerator TearDown()
        {
            Object.Destroy(_anchor);
            Object.Destroy(_ghostTransform);
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

            Press(_keyboard.numpadEnterKey);

            yield return new WaitForFixedUpdate();

            Assert.True(_possession.IsPossessing);
            Assert.AreEqual(AIStateId.Possessed, _monster.stateMachine.currentState);

            Release(_keyboard.numpadEnterKey);
            Press(_keyboard.numpadEnterKey);

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

            Press(_keyboard.numpadEnterKey);

            yield return new WaitForFixedUpdate();

            Release(_keyboard.numpadEnterKey);

            float monsterPreviousPos = _monsterTransform.position.x;

            Press(_keyboard.rightArrowKey);

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

            Press(_keyboard.numpadEnterKey);

            yield return new WaitForFixedUpdate();

            Release(_keyboard.numpadEnterKey);

            float monsterPreviousPos = _monsterTransform.position.x;

            Press(_keyboard.leftArrowKey);

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

            Press(_keyboard.numpadEnterKey);

            yield return new WaitForFixedUpdate();

            Release(_keyboard.numpadEnterKey);

            float monsterPreviousPos = _monsterTransform.position.y;

            Press(_keyboard.upArrowKey);

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

            Press(_keyboard.numpadEnterKey);

            yield return new WaitForFixedUpdate();

            Release(_keyboard.numpadEnterKey);

            float monsterPreviousPos = _monsterTransform.position.y;

            Press(_keyboard.downArrowKey);

            yield return new WaitForFixedUpdate();

            Assert.Less(_monsterTransform.position.y, monsterPreviousPos, 
                "Monster Moved to the down.");
            Assert.True(Utility.FastApproximately(_ghostTransform.position.y, 
                _monsterTransform.position.y, 0.05f), "Ghost is following Monster.");
        }
    }
}