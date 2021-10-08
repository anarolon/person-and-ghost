using System.Collections;
using NUnit.Framework;
using PersonAndGhost.Person;
using PersonAndGhost.Person.States;
using PersonAndGhost.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class GhostlyInvasionTests : InputTestFixture
{
    private GameObject _groundPrefab;
    private GameObject _personPrefab;
    private Transform _groundTransform;
    private Transform _personTransform;
    private PersonMovement _person;
    private Transform _turret;
    private Rigidbody2D _personRigidbody2D;
    private Keyboard _keyboard;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _groundPrefab = Resources.Load<GameObject>(Utility.GROUNDPREFABPATH);
        _personPrefab = Resources.Load<GameObject>(Utility.LEFTPLAYERPREFAB);

        _groundTransform = Object.Instantiate(_groundPrefab).transform;
        _groundTransform.localScale *= 2.5f;

        yield return new EnterPlayMode();
    }

    private IEnumerator PlayModeSetUp()
    {
        PlayerInput personPlayerInput = Utility.InstantiatePlayerWithKeyboard
        (
            _personPrefab,
            default
        );

        var personDevices = personPlayerInput.devices;
        int keyboardIndex = personDevices.Count <= 1 ? 0 :
            personDevices.IndexOf(device => device.GetType() == typeof(Keyboard));

        _personTransform = personPlayerInput.transform;
        _person = _personTransform.GetComponent<PersonMovement>();
        _personRigidbody2D = _person.GetComponent<Rigidbody2D>();
        _turret = _person.GetComponentInChildren<Turret>(true).gameObject.transform;
        _keyboard = (Keyboard) personPlayerInput.devices[keyboardIndex];

        yield return new WaitUntil(() => _person.IsOnGround);
    }

    
    [UnityTearDown]
    public new IEnumerator TearDown()
    {
        Object.Destroy(_groundTransform);
        Object.Destroy(_personTransform);

        yield return new ExitPlayMode();
    }

    [UnityTest]
    public IEnumerator GhostlyInvasionInstantiated()
    {
        yield return PlayModeSetUp();
       
        yield return new WaitForFixedUpdate();

        // enter meditation state
        Press(_keyboard.eKey);
        Release(_keyboard.eKey);

        yield return new WaitUntil(() => _person.IsMeditating);

        // ghostly invasion
        Assert.NotNull(GameObject.FindWithTag(Utility.GHOSTLYINNVASIONTAG));
        Assert.NotNull(GameObject.FindWithTag(Utility.SPIRITTAG));

        // exit meditation
        Press(_keyboard.eKey);
        Release(_keyboard.eKey);

        yield return new WaitForFixedUpdate();

        Assert.Null(GameObject.FindWithTag(Utility.GHOSTLYINNVASIONTAG));
    }

    [UnityTest]
    public IEnumerator PersonAimAndShoot()
    {
        yield return PlayModeSetUp();
       
        yield return new WaitForFixedUpdate();

        // enter meditation state
        Press(_keyboard.eKey);
        Release(_keyboard.eKey);

        yield return new WaitUntil(() => _person.IsMeditating);

        // aim
        GameObject spirit = GameObject.FindWithTag(Utility.SPIRITTAG);
        Vector2 shootPos = spirit.transform.position - _turret.position;
        float angle = Mathf.Atan2(shootPos.y, shootPos.x) * Mathf.Rad2Deg;
        _turret.Rotate(0f, 0f, angle);

        // shoot
        Press(_keyboard.spaceKey);
        yield return new WaitForFixedUpdate();
        Release(_keyboard.spaceKey);
        Assert.False(spirit);

        // exit meditation
        Press(_keyboard.eKey);
        Release(_keyboard.eKey);

        yield return new WaitForFixedUpdate();

        Assert.Null(GameObject.FindWithTag(Utility.GHOSTLYINNVASIONTAG));
    }

}
