using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class GhostPossessionTests
{
    private GameObject _mainCamera;
    private GameObject _ghost;
    private GameObject _monster;
    private GhostMovement _ghostController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _mainCamera = CreateMainCamera();

        _ghost = (GameObject)GameObject.Instantiate((UnityEngine.Object)Resources.Load("Prefabs/Ghost"), Vector3.zero, Quaternion.identity);
        _ghostController = _ghost.GetComponent<GhostMovement>();

        _monster = (GameObject)GameObject.Instantiate((UnityEngine.Object)Resources.Load("Prefabs/Bird"), Vector3.zero, Quaternion.identity);

        yield return new WaitForFixedUpdate();

        _ghostController.Anchor.GetComponent<Rigidbody2D>().gravityScale = 0;

        yield return new EnterPlayMode();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Destroy(_mainCamera);
        GameObject.Destroy(_ghost);
        GameObject.Destroy(_monster);
        yield return new ExitPlayMode();
    }

    [UnityTest]
    public IEnumerator GhostNearMonster()
    {
        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Assert.True(_ghostController.IsNearAMonster);
        Assert.AreEqual(_monster.name, _ghostController.NearbyMonster.gameObject.name);

        yield return new WaitUntil(() => !_ghostController.IsNearAMonster);

        Assert.False(_ghostController.IsNearAMonster);
        Assert.IsNull(_ghostController.NearbyMonster);
    }

    [UnityTest]
    public IEnumerator GhostPossessingMonster()
    {
        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Assert.False(_ghostController.IsPossessing);
        Assert.AreNotEqual(AIStateId.Possessed, _ghostController.NearbyMonster.stateMachine.currentState);

        _ghostController.OnPossession(new InputAction.CallbackContext());

        yield return new WaitForFixedUpdate();

        Assert.True(_ghostController.IsPossessing);
        Assert.AreEqual(AIStateId.Possessed, _ghostController.Monster.stateMachine.currentState);

        _ghostController.OnPossession(new InputAction.CallbackContext());

        yield return new WaitForFixedUpdate();

        Assert.False(_ghostController.IsPossessing);
        Assert.AreNotEqual(AIStateId.Possessed, _ghostController.NearbyMonster.stateMachine.currentState);

        yield return new WaitUntil(() => !_ghostController.IsNearAMonster);

        Assert.False(_ghostController.IsNearAMonster);
    }

    [UnityTest]
    public IEnumerator MoveMonsterPossessedRight()
    {
        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        _ghostController.OnPossession(new InputAction.CallbackContext());

        yield return new WaitForFixedUpdate();

        float monsterPreviousPos = _monster.transform.position.x;

        _ghostController.MovementInput = Vector2.right;

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(1); //Line not needed to run test individually but bug was found when running it with the rest of the tests

        Assert.Greater(_monster.transform.position.x, monsterPreviousPos, "Monster Moved to the right.");
        Assert.True(FastApproximately(_ghost.transform.position.x, _monster.transform.position.x, 0.05f), "Ghost is following Monster.");
    }

    [UnityTest]
    public IEnumerator MoveMonsterPossessedLeft()
    {
        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        _ghostController.OnPossession(new InputAction.CallbackContext());

        yield return new WaitForFixedUpdate();

        float monsterPreviousPos = _monster.transform.position.x;

        _ghostController.MovementInput = Vector2.left;

        yield return new WaitForFixedUpdate();

        Assert.Greater(monsterPreviousPos, _monster.transform.position.x, "Monster Moved to the left.");
        Assert.True(FastApproximately(_ghost.transform.position.x, _monster.transform.position.x, 0.05f), "Ghost is following Monster.");
    }

    [UnityTest]
    public IEnumerator MoveFlyingMonsterPossessedUp()
    {
        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Assert.AreEqual("Bird(Clone)", _ghostController.NearbyMonster.gameObject.name); //Identify flying monster

        _ghostController.OnPossession(new InputAction.CallbackContext());

        yield return new WaitForFixedUpdate();

        float monsterPreviousPos = _monster.transform.position.y;

        _ghostController.MovementInput = Vector2.up;

        yield return new WaitForFixedUpdate();

        Assert.Greater(_monster.transform.position.y, monsterPreviousPos, "Monster Moved to the up.");
        Assert.True(FastApproximately(_ghost.transform.position.y, _monster.transform.position.y, 0.05f), "Ghost is following Monster.");
    }

    [UnityTest]
    public IEnumerator MoveFlyingMonsterPossessedDown()
    {
        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Assert.AreEqual("Bird(Clone)", _ghostController.NearbyMonster.gameObject.name); //Identify flying monster

        _ghostController.OnPossession(new InputAction.CallbackContext());

        yield return new WaitForFixedUpdate();

        float monsterPreviousPos = _monster.transform.position.y;

        _ghostController.MovementInput = Vector2.down;

        yield return new WaitForFixedUpdate();

        Assert.Greater(monsterPreviousPos, _monster.transform.position.y, "Monster Moved to the down.");
        Assert.True(FastApproximately(_ghost.transform.position.y, _monster.transform.position.y, 0.05f), "Ghost is following Monster.");
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

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}