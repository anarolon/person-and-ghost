using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PersonTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void PersonTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PersonLeftMovement()
    {
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Person"));
        PlayerController personController = player.GetComponent<PlayerController>();

        float x = player.transform.position.x;

        personController.MovePlayer(Vector2.left);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new WaitForFixedUpdate();

        Assert.Greater(x, player.transform.position.x, "Move Left");
    }
    [UnityTest]
    public IEnumerator PersonRightMovement()
    {
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Person"));
        PlayerController personController = player.GetComponent<PlayerController>();

        float x = player.transform.position.x;

        personController.MovePlayer(Vector2.right);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new WaitForFixedUpdate();

        Assert.Less(x, player.transform.position.x, "Move Right");
    }

    [UnityTest]
    public IEnumerator PersonJump()
    {
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Person"));
        PlayerController personController = player.GetComponent<PlayerController>();
        player.transform.position = new Vector3(0, 0, 0);

        GameObject ground = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Ground"));
        ground.transform.position = new Vector3(0, -1, 0);

        //Need to Check that onGround is true before the jump. For some reason when i Assert before jumping it ends up false
        // Debug.Log(player.transform.position);
        // Assert.True(personController.IsOnGround());

        personController.Jump();  
        yield return new WaitForFixedUpdate();
        Assert.False(personController.IsOnGround());

        yield return new WaitForSeconds(2);
        Debug.Log(player.transform.position);
        Assert.True(personController.IsOnGround());
    }


    [UnityTest]
    public IEnumerator PersonToolInteract()
    {
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Person"));
        PlayerController personController = player.GetComponent<PlayerController>();
        PlayerToolController personToolController = player.GetComponent<PlayerToolController>();
        player.transform.position = new Vector3(0, 0, 0);

        GameObject climbingGauntlet = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Climbing Gauntlet"));
        ClimbingGauntlet toolScript = climbingGauntlet.GetComponent<ClimbingGauntlet>();
        climbingGauntlet.transform.position = new Vector3(0, 0, 0);

        GameObject ground = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Ground"));
        ground.transform.position = new Vector3(0, -1, 0);

        Assert.False(toolScript.onPlayer);
        personToolController.InteractWithTool(toolScript);

        yield return new WaitForFixedUpdate();
        Assert.True(toolScript.onPlayer);

    }

    [UnityTest]
    public IEnumerator PersonToolAction()
    {
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Person"));
        PlayerController personController = player.GetComponent<PlayerController>();
        PlayerToolController personToolController = player.GetComponent<PlayerToolController>();
        player.transform.position = new Vector3(0, 0, 0);

        GameObject climbingGauntlet = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Climbing Gauntlet"));
        ClimbingGauntlet toolScript = climbingGauntlet.GetComponent<ClimbingGauntlet>();
        climbingGauntlet.transform.position = new Vector3(0, 0, 0);

        GameObject ground = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Ground"));
        ground.transform.position = new Vector3(0, -1, 0);

        GameObject wall = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Wall"));
        wall.transform.position = new Vector3(1, -1, 0);


        personToolController.InteractWithTool(toolScript);
        toolScript.Action();
        yield return new WaitForFixedUpdate();
        Assert.False(personController.IsOnGround());

        yield return new WaitForSeconds(2);
        Assert.True(personController.IsOnGround());


    }

}
