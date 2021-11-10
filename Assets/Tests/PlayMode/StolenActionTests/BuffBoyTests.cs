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
    public class BuffBoyTests : InputTestFixture
    {
        private GameObject _groundPrefab;
        private Transform _groundTransform;
        private GameObject _platformPrefab;
        private Transform _platformTransform;
        private GameObject _buffBoy;
        private Transform _buffBoyTransform;
        private GameObject _ghost;
        private Transform _ghostTransform;
        private GhostPossession _possession;
        private GameObject _anchor;
        private GameObject _cam;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _anchor = new GameObject("Anchor");
            _anchor.AddComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.FreezePositionY;
            _anchor.AddComponent<PersonMovement>();

            _groundPrefab = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            _platformPrefab = Resources.Load<GameObject>(Utility.BREAK_PLATFORM_PREFAB_PATH);
            _buffBoy = Resources.Load<GameObject>(Utility.BUFFBOYPREFABPATH);

            _groundTransform = Object.Instantiate(_groundPrefab).transform;

            _platformTransform = Object.Instantiate(_platformPrefab).transform;
            _platformTransform.position = new Vector3(0, 1, 0);

            _buffBoyTransform = Object.Instantiate(_buffBoy).transform;
            _buffBoyTransform.position = _platformTransform.position + new Vector3(0, 1, 0);

            CameraSetup();

            yield return new WaitForSeconds(1);

            yield return new EnterPlayMode();
        }

        private IEnumerator PlayModeSetUp()
        {
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
            Object.Destroy(_ghostTransform);
            Object.Destroy(_buffBoyTransform);
            Object.Destroy(_anchor);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator BuffBoyBreakPlatformTest()
        {
            yield return PlayModeSetUp();

            Press(Keyboard.current.upArrowKey);
            yield return new WaitUntil(() => _possession.IsNearAMonster);
            Release(Keyboard.current.upArrowKey);

            Press(Keyboard.current.numpadEnterKey);
            Assert.True(_possession.IsPossessing);
            Release(Keyboard.current.numpadEnterKey);

            yield return new WaitForFixedUpdate();

            // stomp //
            Press(Keyboard.current.slashKey);
            Press(Keyboard.current.downArrowKey);
            Release(Keyboard.current.slashKey);
            Release(Keyboard.current.downArrowKey);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            // destroyed //
            Assert.False(_platformTransform);
        }

    }
}
