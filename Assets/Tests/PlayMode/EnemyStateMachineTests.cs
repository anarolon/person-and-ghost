using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyStateMachineTests
{

    [UnityTest]
    public IEnumerator AIBigBoyStateLoopPasses()
    {
        GameObject floor = (GameObject) GameObject.Instantiate((UnityEngine.Object) Resources.Load("Prefabs/Ground"), Vector3.zero, Quaternion.identity);
        GameObject bigBoyObject = (GameObject) GameObject.Instantiate((UnityEngine.Object) Resources.Load("Prefabs/BigBoy"), Vector3.zero, Quaternion.identity);

        AIAgent bigBoyAi = bigBoyObject.GetComponent<AIBigBoy>();  
        // Wait for Set Up
        yield return new WaitForSeconds(1.0f);
        
        AIStateId expectedInitState = AIStateId.Idle;
        Assert.AreEqual(expectedInitState, bigBoyAi.stateMachine.currentState);
        yield return new WaitForSeconds(bigBoyAi.config._doIdleFrequency);
        Assert.AreNotEqual(expectedInitState, bigBoyAi.stateMachine.currentState);
        Assert.AreEqual(AIStateId.MoveX, bigBoyAi.stateMachine.currentState);
        yield return new WaitForSeconds(bigBoyAi.config._changeDirectionFrequency);
        Assert.AreEqual(expectedInitState, bigBoyAi.stateMachine.currentState);

        GameObject.Destroy(bigBoyObject);
        GameObject.Destroy(floor);
    }

    [UnityTest]
    public IEnumerator AIBirdStateLoopPasses()
    {
        GameObject birdObject = (GameObject) GameObject.Instantiate((UnityEngine.Object) Resources.Load("Prefabs/Bird"), Vector3.zero, Quaternion.identity);

        AIAgent birdAi = birdObject.GetComponent<AIBird>();  
        // Wait for Set Up
        yield return new WaitForSeconds(1.0f);
                      
        AIStateId expectedInitState = AIStateId.MoveX;
        Assert.AreEqual(expectedInitState, birdAi.stateMachine.currentState);
        yield return new WaitForSeconds(birdAi.config._changeDirectionFrequency);
        Assert.AreEqual(expectedInitState, birdAi.stateMachine.currentState);

        GameObject.Destroy(birdObject);
    }

    [UnityTest]
    public IEnumerator AIFrogStateLoopPasses()
    {
        GameObject floor = (GameObject) GameObject.Instantiate((UnityEngine.Object) Resources.Load("Prefabs/Ground"), Vector3.zero, Quaternion.identity);
        floor.transform.position = new Vector3(0f, -2);
        GameObject frogObject = (GameObject) GameObject.Instantiate((UnityEngine.Object) Resources.Load("Prefabs/FrogWizard"), Vector3.zero, Quaternion.identity);

        AIAgent frogAi = frogObject.GetComponent<AIFrog>();  
        // Wait for Set Up
        yield return new WaitForSeconds(1.0f);
                      
        AIStateId expectedInitState = AIStateId.Jump;
        Assert.AreEqual(expectedInitState, frogAi.stateMachine.currentState);
        yield return new WaitForSeconds(frogAi.config._doJumpFrequency);
        Assert.AreEqual(expectedInitState, frogAi.stateMachine.currentState);

        GameObject.Destroy(frogObject);
        GameObject.Destroy(floor);
    }
}
