using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using PersonAndGhost.Utils;
using UnityEngine.SceneManagement;

namespace PersonAndGhost.PlayMode
{
    public class PauseMenuTests
    {
        // This test does not play well with others
        //private string CurrentSceneName => SceneManager.GetActiveScene().name;
        //private string FirstSceneName => SceneManager.GetSceneByBuildIndex(0).name;
        
        //[UnityTest]
        //public IEnumerator RoomResetTest()
        //{
        //    SceneManager.LoadSceneAsync(0);

        //    yield return new WaitUntil(() => CurrentSceneName.Contains(FirstSceneName));

        //    string sceneName;
        //    PauseMenuController pauseGameUI;

        //    sceneName = CurrentSceneName;
        //    pauseGameUI = Object.FindObjectOfType<PauseMenuController>();

        //    yield return new WaitForFixedUpdate();

        //    Actions.OnGamePause();

        //    Assert.Zero(Time.timeScale);

        //    pauseGameUI.RoomReset();

        //    yield return new WaitForSecondsRealtime(
        //        Object.FindObjectOfType<GameManagerController>()
        //        .timeToWaitBeforeLoadingScene + 1);

        //    Assert.AreEqual(sceneName, CurrentSceneName);
        //    Assert.Positive(Time.timeScale);
        //}
        
        [UnityTest]
        public IEnumerator TimeScaleTest()
        {
            GameObject gameUIPrefab;
            GameObject gameManagPrefab;
            GameObject gameUIInstance;
            GameObject gameManagInstance;
            PauseMenuController pauseGameUI;

            gameUIPrefab = Resources.Load<GameObject>(Utility.GAMEUIPREFABPATH);
            gameUIInstance = Object.Instantiate(gameUIPrefab);
            pauseGameUI = gameUIInstance.GetComponentInChildren<PauseMenuController>();
            gameManagPrefab = Resources.Load<GameObject>(Utility.GAMEMANAGERPREFABPATH);
            gameManagInstance = Object.Instantiate(gameManagPrefab);

            yield return new WaitForFixedUpdate();

            Assert.NotZero(Time.timeScale);

            Actions.OnGamePause();

            Assert.Zero(Time.timeScale);

            pauseGameUI.HandleGameUnPause();
            
            yield return new WaitForFixedUpdate();
            
            Assert.Positive(Time.timeScale);

            Object.Destroy(gameUIInstance);
            Object.Destroy(gameManagInstance);
        }
    }
}
