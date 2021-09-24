using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PersonTests
{
    private GameObject _ground;
    private GameObject _person;
    private PlayerController _personController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _ground = (GameObject)GameObject.Instantiate((UnityEngine.Object)Resources.Load("Prefabs/Ground"));
        _ground.transform.localScale *= 2.5f;

        _person = (GameObject)GameObject.Instantiate((UnityEngine.Object)Resources.Load("Prefabs/Person"), Vector3.zero, Quaternion.identity);
        _personController = _person.GetComponent<PlayerController>();

        yield return new WaitUntil(() => _personController.IsOnGround);

        yield return new EnterPlayMode();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Destroy(_ground);
        GameObject.Destroy(_person);

        yield return new ExitPlayMode();
    }

    [UnityTest]
    public IEnumerator MoveLeft()
    {
        var previousPos = _person.transform.position.x;
        _personController.MovementInput = Vector2.left;
        yield return new WaitForFixedUpdate();
        Assert.Greater(previousPos, _person.transform.position.x, "Moved to the left.");
    }

    [UnityTest]
    public IEnumerator MoveRight()
    {
        var previousPos = _person.transform.position.x;
        _personController.MovementInput = Vector2.right;
        yield return new WaitForFixedUpdate();
        Assert.Greater(_person.transform.position.x, previousPos, "Moved to the right.");
    }

    [UnityTest]
    public IEnumerator Jump()
    {
        _personController.IsJumping = true;
        yield return new WaitUntil(() => !_personController.IsOnGround);
        Assert.False(_personController.IsOnGround);
        yield return new WaitUntil(() => _personController.IsOnGround);
        Assert.True(_personController.IsOnGround);
    }


    [UnityTest]
    public IEnumerator ToolPickUpAndDrop()
    {
        GameObject tool = (GameObject)GameObject.Instantiate((UnityEngine.Object)Resources.Load("Prefabs/Climbing Gauntlet"), Vector3.zero, Quaternion.identity);

        yield return new WaitForFixedUpdate();

        PlayerToolController personToolController = _person.GetComponent<PlayerToolController>();
        ClimbingGauntlet toolScript = tool.GetComponent<ClimbingGauntlet>();

        Assert.False(toolScript.OnPlayer);

        personToolController.InteractWithTool(toolScript);

        yield return new WaitForFixedUpdate();

        Assert.True(toolScript.OnPlayer);

        personToolController.InteractWithTool(toolScript);

        yield return new WaitForFixedUpdate();

        Assert.False(toolScript.OnPlayer);

        GameObject.Destroy(tool);
    }

    [UnityTest]
    public IEnumerator ToolActionClimbingGauntlet()
    {
        GameObject tool = (GameObject)GameObject.Instantiate((UnityEngine.Object)Resources.Load("Prefabs/Climbing Gauntlet"), Vector3.zero, Quaternion.identity);
        GameObject wall = (GameObject)GameObject.Instantiate((UnityEngine.Object)Resources.Load("Prefabs/Wall"), new Vector3(1, 3, 0), Quaternion.identity);

        yield return new WaitForFixedUpdate();

        PlayerToolController personToolController = _person.GetComponent<PlayerToolController>();
        ClimbingGauntlet toolScript = tool.GetComponent<ClimbingGauntlet>();

        personToolController.InteractWithTool(toolScript);
        toolScript.Action();

        yield return new WaitUntil(() => !_personController.IsOnGround);
        Assert.False(_personController.IsOnGround);

        yield return new WaitUntil(() => _personController.IsOnGround);
        Assert.True(_personController.IsOnGround);

        GameObject.Destroy(tool);
        GameObject.Destroy(wall);
    }
}
