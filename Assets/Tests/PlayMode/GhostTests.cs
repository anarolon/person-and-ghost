using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GhostTests
{
    private GameObject _mainCamera;
    private GameObject _ghost;
    private GhostMovement _ghostController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _mainCamera = CreateMainCamera();

        _ghost = (GameObject) GameObject.Instantiate((UnityEngine.Object) Resources.Load("Prefabs/Ghost"), Vector3.zero, Quaternion.identity);
        _ghostController = _ghost.GetComponent<GhostMovement>();

        yield return new WaitForFixedUpdate();
        
        _ghostController.Anchor.GetComponent<Rigidbody2D>().gravityScale = 0;

        yield return new EnterPlayMode();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Destroy(_mainCamera);
        GameObject.Destroy(_ghost);
        yield return new ExitPlayMode();
    }

    [UnityTest]
    public IEnumerator MoveUp()
    {
        var previousPos = _ghost.transform.position.y;
        _ghostController.MovementInput = Vector2.up;
        yield return new WaitForFixedUpdate();
        Assert.Greater(_ghost.transform.position.y, previousPos, "Moved upwards.");
    }

    [UnityTest]
    public IEnumerator MoveDown()
    {
        var previousPos = _ghost.transform.position.y;
        _ghostController.MovementInput = Vector2.down;
        yield return new WaitForFixedUpdate();
        Assert.Greater(previousPos, _ghost.transform.position.y, "Moved downwards.");
    }

    [UnityTest]
    public IEnumerator MoveLeft()
    {
        var previousPos = _ghost.transform.position.x;
        _ghostController.MovementInput = Vector2.left;
        yield return new WaitForFixedUpdate();
        Assert.Greater(previousPos, _ghost.transform.position.x, "Moved to the left.");
    }

    [UnityTest]
    public IEnumerator MoveRight()
    {
        float previousPos = _ghost.transform.position.x;
        _ghostController.MovementInput = Vector2.right;
        yield return new WaitForFixedUpdate();
        Assert.Greater(_ghost.transform.position.x, previousPos, "Moved to the right.");
    }

    [UnityTest]
    public IEnumerator AnchorBackToPlayer()
    {
        var ghostRB = _ghost.GetComponent<Rigidbody2D>();
        var offset = _ghostController.AnchorRBRange + 5.0f;
        var previousPos = ghostRB.position;

        _ghostController.Anchor.position = Vector2.one * offset;

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
            
        Assert.AreEqual(_ghostController.Anchor.position, ghostRB.position, "Ghost is anchored back to Anchor.");
        Assert.AreNotEqual(previousPos, ghostRB.position, "Ghost exceeded the anchor range.");

        ghostRB.MovePosition(Vector2.right * offset);

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(_ghostController.Anchor.position, ghostRB.position, "Ghost is anchored back to Anchor.");
        Assert.AreNotEqual(previousPos, ghostRB.position, "Ghost exceeded the anchor range.");
    }

    private static GameObject CreateMainCamera()
    {
        var camera = new GameObject
        {
            name = "Main Camera",
            tag = "MainCamera"
        };

        camera.AddComponent<Camera>().backgroundColor = Color.blue;
        camera.transform.position = new Vector3(0, 0, -10);

        return camera;
    }
}

