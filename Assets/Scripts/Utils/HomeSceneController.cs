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
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneName);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
