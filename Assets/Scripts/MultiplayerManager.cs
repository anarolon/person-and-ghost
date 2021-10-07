using UnityEngine;
using PersonAndGhost.Utils;

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

            if (!_leftPlayerPrefab)
            {
                _leftPlayerPrefab = Resources.Load<GameObject>(Utility.LEFTPLAYERPREFAB);
            }

            leftPlayerTransform = Utility.InstantiatePlayerWithKeyboard(
                _leftPlayerPrefab, _leftPlayerPosition).gameObject.transform;

            rightPlayerTransform = _rightPlayerPrefab
                ? Utility.InstantiatePlayerWithKeyboard(
                    _rightPlayerPrefab, _rightPlayerPosition).gameObject.transform
                : Utility.RightPlayerManualInstantiation(_rightPlayerPosition).transform;

            leftPlayerTransform.parent = gameObject.transform;
            rightPlayerTransform.parent = gameObject.transform;
        }

    }
}
