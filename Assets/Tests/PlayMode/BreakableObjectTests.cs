using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PersonAndGhost.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace PersonAndGhost.PlayMode
{
    public class TriggerClass : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other) {
            Breakable _breakable = other.gameObject.GetComponent<Breakable>();
            if(_breakable) {
                _breakable.Break();
            }
        }
    }

    public class BreakableObjectTests : InputTestFixture
    {
        private GameObject _groundPrefab;
        private Transform _groundTransform;
        private GameObject _platformPrefab;
        private Transform _platformTransform;
        private GameObject _destroyer;
        private Rigidbody2D _rb;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _groundPrefab = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
            _platformPrefab = Resources.Load<GameObject>(Utility.BREAK_PLATFORM_PREFAB_PATH);

            _groundTransform = Object.Instantiate(_groundPrefab).transform;

            _platformTransform = Object.Instantiate(_platformPrefab).transform;
            _platformTransform.position = new Vector3(0, 1, 0);

            _destroyer = new GameObject();
            _rb = _destroyer.AddComponent<Rigidbody2D>();
            _destroyer.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Square");
            _destroyer.AddComponent<BoxCollider2D>();
            _destroyer.AddComponent<TriggerClass>();

            yield return new EnterPlayMode();
        }

        [UnityTearDown]
        public new IEnumerator TearDown()
        {
            Object.Destroy(_groundTransform);
            Object.Destroy(_destroyer);

            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator BreakObjectTest()
        {
            yield return new WaitForFixedUpdate();
            _rb.MovePosition(_platformTransform.position + Vector3.up);     
            // wait for object falling       
            yield return new WaitForSeconds(2);
            Assert.False(_platformTransform);

        }
    }
}
