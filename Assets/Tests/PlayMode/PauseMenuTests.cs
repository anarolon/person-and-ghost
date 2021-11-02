using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using PersonAndGhost.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace PersonAndGhost.PlayMode
{
    public class PauseMenuTests
    {
        private string CurrentSceneName => SceneManager.GetActiveScene().name;
        private int sceneToLoad = 0;

        private GameObject _gameUIPrefab = default;
        private GameObject _gameManagerPrefab = default;
        private PauseMenuController _pauseGameUI = default;


        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _gameUIPrefab
                = Resources.Load<GameObject>(Utility.GAMEUIPREFABPATH);

            _gameManagerPrefab
                = Resources.Load<GameObject>(Utility.GAMEMANAGERPREFABPATH);

            yield return new EnterPlayMode();
        }
        private IEnumerator PlayModeSetUp()
        {
            GameObject _gameUIInstance = Object.Instantiate(_gameUIPrefab);
            _pauseGameUI = _gameUIInstance.transform.GetComponentInChildren<PauseMenuController>();

            Object.Instantiate(_gameManagerPrefab);

            yield return new WaitForFixedUpdate();

        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            //Object.Destroy(_gameUIPrefab);
            yield return new ExitPlayMode();
        }


        [UnityTest]
        public IEnumerator TimeScaleTest()
        {
            yield return PlayModeSetUp();
            Actions.OnGamePause();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(Time.timeScale, 0);

            _pauseGameUI.HandleGameUnPause();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(Time.timeScale, 1);
        }

        // TODO: Figure out how to unload the scene so that the other test runs appropriately
        [UnityTest]
        public IEnumerator ZRoomResetTest()
        {
            SceneManager.LoadSceneAsync(sceneToLoad);
            yield return new WaitForSeconds(1);

            string sceneName = CurrentSceneName;
           GameObject.FindObjectOfType<PauseMenuController>().RoomReset();
            yield return new WaitForSeconds(1);
            Assert.AreEqual(sceneName, CurrentSceneName);

            SceneManager.UnloadSceneAsync(sceneToLoad);
            yield return new ExitPlayMode();



        }
        

        
    }
}
