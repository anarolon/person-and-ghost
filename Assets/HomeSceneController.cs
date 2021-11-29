using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PersonAndGhost
{
    public class HomeSceneController : MonoBehaviour
    {

        public void LoadSceneFromText(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
