using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using PersonAndGhost.Utils;
using UnityEngine.SceneManagement;

namespace PersonAndGhost.PlayMode
{
    public class RoomManagerTests
    {
        private int SceneAmount => SceneManager.sceneCountInBuildSettings - 1;
        private string FirstSceneName => SceneManager.GetSceneByBuildIndex(1).name;
        private string CurrentSceneName => SceneManager.GetActiveScene().name;
        private string LastSceneName => 
            SceneManager.GetSceneByBuildIndex(SceneAmount - 1).name;

        [UnityTest]
        public IEnumerator TransitionFromFirstSceneToLast()
        {
            yield return TestTransitionFromFirstSceneToItself();

            if (SceneAmount > 1)
            {
                yield return TestTransitionToNextScene();
            }

            yield return TestTransitionFromLastSceneToFloorWin();
        }

        private IEnumerator TestTransitionFromFirstSceneToItself()
        {
            SceneManager.LoadSceneAsync(1);

            yield return new WaitUntil(() => CurrentSceneName.Contains(FirstSceneName));

            yield return SceneTransitionHelper(false);

            Assert.AreEqual(FirstSceneName, CurrentSceneName);
        }

        private IEnumerator TestTransitionToNextScene()
        {
            yield return SceneTransitionHelper(true);

            Assert.AreNotEqual(FirstSceneName, CurrentSceneName);
        }

        private IEnumerator TestTransitionFromLastSceneToFloorWin()
        {
            SceneManager.LoadSceneAsync(SceneAmount - 1);

            yield return new WaitUntil(() => CurrentSceneName.Contains(LastSceneName));

            Assert.NotZero(Time.timeScale);

            Actions.OnFloorStateChange(true);

            yield return new WaitForSecondsRealtime(
                Object.FindObjectOfType<GameManagerController>()
                .timeToWaitBeforeLoadingScene + 1);

            Assert.Zero(Time.timeScale);
        }

        private static IEnumerator SceneTransitionHelper(bool state)
        {
            Actions.OnRoomStateChange(state);

            yield return new WaitForSecondsRealtime(
                Object.FindObjectOfType<GameManagerController>()
                .timeToWaitBeforeLoadingScene + 1);
        }
    }
}