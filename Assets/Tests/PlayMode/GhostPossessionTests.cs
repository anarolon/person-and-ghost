using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class GhostPossessionTests : InputTestFixture
{
    private GameObject _mainCamera;
    private GameObject _ghost;
    private GameObject _monster;
    private GhostMovement _ghostController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _mainCamera = CreateMainCamera();

        _monster = Resources.Load<GameObject>("Prefabs/Bird");
        _ghost = Resources.Load<GameObject>("Prefabs/Ghost");

        _monster = MonoBehaviour.Instantiate(_monster);

        yield return new EnterPlayMode();
    }

    [UnityTearDown]
    public new IEnumerator TearDown()
    {
        GameObject.Destroy(_mainCamera);
        GameObject.Destroy(_ghost);
        GameObject.Destroy(_monster);
        yield return new ExitPlayMode();
    }

    [UnityTest]
    public IEnumerator GhostNearMonster()
    {
        InstantiateGhostWithKeyboard();

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
        Keyboard keyboard = InstantiateGhostWithKeyboard();

        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Assert.False(_ghostController.IsPossessing);
        Assert.AreNotEqual(AIStateId.Possessed, _ghostController.NearbyMonster.stateMachine.currentState);

        Press(keyboard.numpadEnterKey);

        yield return new WaitForFixedUpdate();

        Assert.True(_ghostController.IsPossessing);
        Assert.AreEqual(AIStateId.Possessed, _ghostController.Monster.stateMachine.currentState);

        Release(keyboard.numpadEnterKey);
        Press(keyboard.numpadEnterKey);

        yield return new WaitForFixedUpdate();

        Assert.False(_ghostController.IsPossessing);
        Assert.AreNotEqual(AIStateId.Possessed, _ghostController.NearbyMonster.stateMachine.currentState);

        yield return new WaitUntil(() => !_ghostController.IsNearAMonster);

        Assert.False(_ghostController.IsNearAMonster);
    }

    [UnityTest]
    public IEnumerator MoveMonsterPossessedRight()
    {
        Keyboard keyboard = InstantiateGhostWithKeyboard();

        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Press(keyboard.numpadEnterKey);

        yield return new WaitForFixedUpdate();

        Release(keyboard.numpadEnterKey);

        float monsterPreviousPos = _monster.transform.position.x;

        Press(keyboard.rightArrowKey);

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(1); //Line not needed to run test individually but bug was found when running it with the rest of the tests

        Assert.Greater(_monster.transform.position.x, monsterPreviousPos, "Monster Moved to the right.");
        Assert.True(FastApproximately(_ghost.transform.position.x, _monster.transform.position.x, 0.05f), "Ghost is following Monster.");
    }

    [UnityTest]
    public IEnumerator MoveMonsterPossessedLeft()
    {
        Keyboard keyboard = InstantiateGhostWithKeyboard();

        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Press(keyboard.numpadEnterKey);

        yield return new WaitForFixedUpdate();

        Release(keyboard.numpadEnterKey);

        float monsterPreviousPos = _monster.transform.position.x;

        Press(keyboard.leftArrowKey);

        yield return new WaitForFixedUpdate();

        Assert.Greater(monsterPreviousPos, _monster.transform.position.x, "Monster Moved to the left.");
        Assert.True(FastApproximately(_ghost.transform.position.x, _monster.transform.position.x, 0.05f), "Ghost is following Monster.");
    }

    [UnityTest]
    public IEnumerator MoveFlyingMonsterPossessedUp()
    {
        Keyboard keyboard = InstantiateGhostWithKeyboard();

        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Assert.AreEqual("Bird(Clone)", _ghostController.NearbyMonster.gameObject.name); //Identify flying monster

        Press(keyboard.numpadEnterKey);

        yield return new WaitForFixedUpdate();

        Release(keyboard.numpadEnterKey);

        float monsterPreviousPos = _monster.transform.position.y;

        Press(keyboard.upArrowKey);

        yield return new WaitForFixedUpdate();

        Assert.Greater(_monster.transform.position.y, monsterPreviousPos, "Monster Moved to the up.");
        Assert.True(FastApproximately(_ghost.transform.position.y, _monster.transform.position.y, 0.05f), "Ghost is following Monster.");
    }

    [UnityTest]
    public IEnumerator MoveFlyingMonsterPossessedDown()
    {
        Keyboard keyboard = InstantiateGhostWithKeyboard();

        yield return new WaitUntil(() => _ghostController.IsNearAMonster);

        Assert.AreEqual("Bird(Clone)", _ghostController.NearbyMonster.gameObject.name); //Identify flying monster

        Press(keyboard.numpadEnterKey);

        yield return new WaitForFixedUpdate();

        Release(keyboard.numpadEnterKey);

        float monsterPreviousPos = _monster.transform.position.y;

        Press(keyboard.downArrowKey);

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

    private Keyboard InstantiateGhostWithKeyboard()
    {
        Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

        _ghost = PlayerInput.Instantiate
        (
            _ghost,
            controlScheme: "KeyboardRight",
            pairWithDevice: keyboard
        ).gameObject;

        _ghostController = _ghost.GetComponent<GhostMovement>();

        return keyboard;
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}