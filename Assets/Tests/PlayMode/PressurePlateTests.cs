using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using PersonAndGhost.Utils;
using System;
namespace PersonAndGhost.PlayMode
{
    public class PressurePlateTests
    {
        [UnityTest]
        public IEnumerator PersonOpensHatchDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.HATCHDOORPREFAB,
                typeof(HatchDoor));

            yield return TestPressurePlateTrigger(Utility.LEFTPLAYERTAG,
                objectList[0], objectList[1], false);
        }

        [UnityTest]
        public IEnumerator PersonOpensNormalDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.NORMALDOORPREFAB,
                typeof(NormalDoor));

            yield return TestPressurePlateTrigger(Utility.LEFTPLAYERTAG,
                objectList[0], objectList[1], false);
        }

        [UnityTest]
        public IEnumerator PossessedMonsterOpensHatchDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.HATCHDOORPREFAB,
                typeof(HatchDoor));

            yield return TestPressurePlateTrigger(Utility.MONSTERTAG,
                objectList[0], objectList[1], false);
        }

        [UnityTest]
        public IEnumerator PossessedMonsterOpensNormalDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.NORMALDOORPREFAB,
                typeof(NormalDoor));

            yield return TestPressurePlateTrigger(Utility.MONSTERTAG,
                objectList[0], objectList[1], false);
        }

        [UnityTest]
        public IEnumerator UnpossessedMonsterOpensHatchDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.HATCHDOORPREFAB,
                typeof(HatchDoor));

            yield return TestPressurePlateTrigger(Utility.MONSTERTAG,
                objectList[0], objectList[1], true);
        }

        [UnityTest]
        public IEnumerator UnpossessedMonsterOpensNormalDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.NORMALDOORPREFAB,
                typeof(NormalDoor));

            yield return TestPressurePlateTrigger(Utility.MONSTERTAG,
                objectList[0], objectList[1], true);
        }

        private GameObject[] PlayModeSetUp(string doorPrefab, Type doorType)
        {
            GameObject pressurePlate = SetUpPressurePlate(doorPrefab);
            GameObject door = Utility.GetChildOfGivenType(pressurePlate, doorType);

            return new GameObject[] { pressurePlate, door };
        }

        private GameObject SetUpPressurePlate(string doorPrefab)
        {
            GameObject prefab = Resources.Load<GameObject>(doorPrefab);
            return GameObject.Instantiate(prefab);
        }

        private IEnumerator TestPressurePlateTrigger(string tagToTest, 
            GameObject pressurePlate, GameObject door, bool isUnpossessedMonster)
        {
            Assert.IsNotNull(door, "Assert door is not opened");

            GameObject dropped = DropObjectOnTrigger
            (
                tagToTest, 
                Utility.GetChildOfGivenType(pressurePlate, typeof(PressurePlateTrigger)),
                isUnpossessedMonster
            );

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            if (isUnpossessedMonster)
            {
                Assert.False(door == null, "Assert door remains unopened");
            }

            else
            {
                Assert.True(door == null, "Assert door is opened");
            }

            GameObject.Destroy(dropped);
            GameObject.Destroy(pressurePlate);
        }

        private GameObject DropObjectOnTrigger(string tagToTest, GameObject trigger,
            bool isUnpossessedMonster)
        {
            GameObject result = new GameObject(tagToTest,
                new Type[] { typeof(BoxCollider2D), typeof(Rigidbody2D) })
            {
                tag = tagToTest
            };

            if (!isUnpossessedMonster && tagToTest == Utility.MONSTERTAG)
            {
                result.AddComponent<AIAgent>().isPossessed = true;
            }

            result.transform.position = trigger.transform.position;

            return result;
        }
    }
}