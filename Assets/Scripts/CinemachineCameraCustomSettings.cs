using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineCameraCustomSettings : MonoBehaviour
{
    [SerializeField] private string _transformToFollowTag = "Person";
    private bool _errorFlag = false;
    private void Start()
    {
        FindTransformToFollow();
    }
    private void FixedUpdate()
    {
        if (_errorFlag)
        {
            FindTransformToFollow();
        }
    }

    private void FindTransformToFollow()
    {
        var gameObjectToFollow = GameObject.FindGameObjectWithTag(_transformToFollowTag);
        try
        {
            GetComponent<CinemachineVirtualCamera>().Follow = gameObjectToFollow.transform;
            _errorFlag = false;
        }
        catch
        {
            _errorFlag = true;
            Debug.LogWarning("The game object with tag " + _transformToFollowTag
                + " was not found in the scene. Search result was Null. "
                + "Camera will be static. Until FixedUpdate where the script will try to search the transform until found.");
        }
    }
}