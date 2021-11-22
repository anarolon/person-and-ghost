using NUnit.Framework;
using PersonAndGhost.Utils;
using System.Linq;
using UnityEngine;

namespace PersonAndGhost.EditMode
{
    public class ClipsTest
    {
        [Test]
        public void AudioManagerPrefabIsInLineWithClips()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/AudioManager");

            AudioClip[] array = prefab.GetComponent<AudioManagerController>().AudioClips;

            int clipsLength =
                (int)System.Enum.GetValues(typeof(Clips)).Cast<Clips>().Max() + 1;

            bool hasEveryClip = false;

            if (array != null && array.Length == clipsLength)
            {
                //Debug.Log("Array has elements required.");
                for (int index = 0; index < clipsLength; index++)
                {
                    //Debug.Log("Looking for: " + ((Clips)index).ToString().ToLower());
                    foreach (AudioClip clip in array)
                    {
                        //Debug.Log("Inside of: " + clip.name.ToLower());
                        if (clip.name.ToLower()
                            .Contains(((Clips)index).ToString().ToLower()))
                        {
                            hasEveryClip = true;
                            //Debug.Log("hasEveryClip: " + hasEveryClip);
                            break;
                        }
                        else
                        {
                            hasEveryClip = false;
                            //Debug.Log("hasEveryClip: " + hasEveryClip);
                        }
                    }
                }
            }

            //else
            //{
            //    Debug.LogWarning("Array is null or different size than required. \n" +
            //        "Array: " + array);
            //}

            Assert.IsTrue(hasEveryClip);
        }
    }
}