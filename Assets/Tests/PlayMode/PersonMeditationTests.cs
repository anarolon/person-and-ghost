using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PersonMeditationTests
{
    private GameObject _mainCamera;
    private GameObject _ground;
    private GameObject _person;
    private GameObject _ghost;
    private PlayerController _personController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _mainCamera = CreateMainCamera();

        _ground = (GameObject)GameObject.Instantiate((UnityEngine.Object)Resources.Load("Prefabs/Ground"));
        _ground.transform.localScale *= 2.5f;

        _person = Resources.Load<GameObject>("Prefabs/Person");
        _ghost = Resources.Load<GameObject>("Prefabs/Ghost");

        _person = MonoBehaviour.Instantiate(_person, Vector3.zero, Quaternion.identity);
        _personController = _person.GetComponent<PlayerController>();

        _person.GetComponent<PersonAndGhost.DestroyWhenOutOfScreen>().enabled = false;
        _person.GetComponent<PersonCollectableController>().enabled = false;

        yield return new WaitUntil(() => _personController.IsOnGround);

        yield return new EnterPlayMode();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Destroy(_mainCamera);
        GameObject.Destroy(_ground);
        GameObject.Destroy(_person);

        yield return new ExitPlayMode();
    }

    [UnityTest]
    public IEnumerator StopWhenPersonIsMeditating()
    {
        var personRB = _person.GetComponent<Rigidbody2D>();

        _personController.MovementInput = Vector2.left;

        yield return new WaitForFixedUpdate();

        Assert.AreNotEqual(Vector2.zero, personRB.velocity);

        _personController.IsMeditating = true;

        yield return new WaitUntil(() => personRB.velocity == Vector2.zero);

        Assert.AreEqual(Vector2.zero, personRB.velocity);

        _personController.IsJumping = true;
        _personController.MovementInput = Vector2.right;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(Vector2.zero, personRB.velocity);

        _personController.IsMeditating = false;

        yield return new WaitForFixedUpdate();

        Assert.AreNotEqual(Vector2.zero, personRB.velocity);
    }

    [UnityTest]
    public IEnumerator IncreaseGhostRangeWhenPersonIsMeditating()
    {
        _ghost = MonoBehaviour.Instantiate(_ghost, Vector3.one, Quaternion.identity);
        _mainCamera.SetActive(true);

        yield return new WaitForSeconds(1);
        
        var ghostController = _ghost.GetComponent<GhostMovement>();
        var ghostRB = _ghost.GetComponent<Rigidbody2D>();
        var offset = ghostController.AnchorRBRange * 1.5f;

        ghostRB.MovePosition(Vector2.one * offset);

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Assert.True(FastApproximately(ghostController.Anchor.position.x, ghostRB.position.x, 0.005f), "Ghost is anchored back to Person in x-axis.");
        Assert.True(FastApproximately(ghostController.Anchor.position.y, ghostRB.position.y, 0.005f), "Ghost is anchored back to Person in y-axis.");

        _personController.IsMeditating = true;

        yield return new WaitForFixedUpdate();

        ghostRB.MovePosition(Vector2.one * offset);

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Assert.False(FastApproximately(ghostController.Anchor.position.x, ghostRB.position.x, 0.005f), "Ghost is not anchored back to Person in x-axis.");
        Assert.False(FastApproximately(ghostController.Anchor.position.y, ghostRB.position.y, 0.005f), "Ghost is not anchored back to Person in y-axis.");

        _personController.IsMeditating = false;

        yield return new WaitForFixedUpdate();

        Assert.True(FastApproximately(ghostController.Anchor.position.x, ghostRB.position.x, 0.005f), "Ghost is anchored back to Person in x-axis.");
        Assert.True(FastApproximately(ghostController.Anchor.position.y, ghostRB.position.y, 0.005f), "Ghost is anchored back to Person in y-axis.");

        GameObject.Destroy(_ghost);
    }

    private static GameObject CreateMainCamera()
    {
        var camera = new GameObject
        {
            name = "Main Camera",
            tag = "MainCamera"
        };

        camera.AddComponent<Camera>().backgroundColor = Color.blue;
        camera.transform.position = new Vector3(0, 0, -20);
        camera.SetActive(false);

        return camera;
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
