using UnityEngine;
using UnityEngine.InputSystem;

namespace PersonAndGhost
{
    public class MultiplayerManager : MonoBehaviour
    {
        [Header("Left Player Fields")]
        [SerializeField] private GameObject _leftPlayerPrefab = default;
        [SerializeField] private Vector2 _leftPlayerPosition = Vector2.one * 2;

        [Header("Right Player Fields")]
        [SerializeField] private GameObject _rightPlayerPrefab = default;
        [SerializeField] private Vector2 _rightPlayerPosition = -Vector2.one;

        private void Start()
        {
            Transform leftPlayerTransform;
            Transform rightPlayerTransform;

            try
            {
                leftPlayerTransform = PlayerInput.Instantiate
                (
                    _leftPlayerPrefab,
                    controlScheme: "KeyboardLeft",
                    pairWithDevice: Keyboard.current
                ).gameObject.transform;

                rightPlayerTransform = PlayerInput.Instantiate
                    (
                        _rightPlayerPrefab,
                        controlScheme: "KeyboardRight",
                        pairWithDevice: Keyboard.current
                    ).gameObject.transform;
            }

            catch (System.ArgumentNullException exception)
            {
                GameObject leftPlayerGameObject;
                GameObject rightPlayerGameObject;

                leftPlayerGameObject = Resources.Load<GameObject>("Prefabs/Person");
                rightPlayerGameObject = Resources.Load<GameObject>("Prefabs/Ghost");

                leftPlayerGameObject = PlayerInput.Instantiate
                (
                    leftPlayerGameObject,
                    controlScheme: "KeyboardLeft",
                    pairWithDevice: Keyboard.current
                ).gameObject;

                rightPlayerGameObject = PlayerInput.Instantiate
                    (
                        rightPlayerGameObject,
                        controlScheme: "KeyboardRight",
                        pairWithDevice: Keyboard.current
                    ).gameObject;

                leftPlayerTransform = leftPlayerGameObject.transform;
                rightPlayerTransform = rightPlayerGameObject.transform;

                Debug.LogException(exception, this);
            }


            leftPlayerTransform.parent = this.gameObject.transform;
            rightPlayerTransform.parent = this.gameObject.transform;

            leftPlayerTransform.position = _leftPlayerPosition;
            rightPlayerTransform.position = _rightPlayerPosition;
        }
    }
}
