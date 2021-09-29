using UnityEngine;

namespace PersonAndGhost.Utils
{
    public static class Utility
    {
        //To be visible, the object most be between 0 and 1 for both X and Y positions
        public static bool IsVisibleToCamera(Camera mainCamera, Vector3 objectPosition)
        {
            Vector3 cameraVision = mainCamera.WorldToViewportPoint(objectPosition);
            return (cameraVision.x >= 0 && cameraVision.y >= 0)
                    && (cameraVision.x <= 1 && cameraVision.y <= 1);
        }
    }
}