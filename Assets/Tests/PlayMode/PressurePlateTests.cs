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
                objectList[0], objectList[1]);
        }

        [UnityTest]
        public IEnumerator MonsterOpensHatchDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.HATCHDOORPREFAB,
                typeof(HatchDoor));

            yield return TestPressurePlateTrigger(Utility.MONSTERTAG,
                objectList[0], objectList[1]);
        }

        [UnityTest]
        public IEnumerator PersonOpensNormalDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.NORMALDOORPREFAB,
                typeof(NormalDoor));

            yield return TestPressurePlateTrigger(Utility.LEFTPLAYERTAG,
                objectList[0], objectList[1]);
        }

        [UnityTest]
        public IEnumerator MonsterOpensNormalDoor()
        {
            GameObject[] objectList = PlayModeSetUp(Utility.NORMALDOORPREFAB,
                typeof(NormalDoor));

            yield return TestPressurePlateTrigger(Utility.MONSTERTAG,
                objectList[0], objectList[1]);
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
            GameObject pressurePlate, GameObject door)
        {
            Assert.IsNotNull(door);

            GameObject dropped = DropObjectOnTrigger
            (
                tagToTest, 
                Utility.GetChildOfGivenType(pressurePlate, typeof(PressurePlateTrigger))
            );

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            Assert.True(door == null);

            GameObject.Destroy(dropped);
            GameObject.Destroy(pressurePlate);
        }

        private GameObject DropObjectOnTrigger(string tagToTest, GameObject trigger)
        {
            GameObject result = new GameObject(tagToTest,
                new Type[] { typeof(BoxCollider2D), typeof(Rigidbody2D) })
            {
                tag = tagToTest
            };

            result.transform.position = trigger.transform.position;

            return result;
        }
    }
}