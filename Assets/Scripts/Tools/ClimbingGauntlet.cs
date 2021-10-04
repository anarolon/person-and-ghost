using UnityEngine;

public class ClimbingGauntlet : Tool
{
    private PersonController _obtainerController = default;
    private StateMachine _obtainerStateMachine = default;

    protected override void ToolAction(GameObject obtainer)
    {
        base.ToolAction(obtainer);

        if (_obtainerController != null && _obtainerStateMachine != null
            && _obtainerController.IsTouchingWall)
        {
            _obtainerStateMachine.ChangeState(_obtainerController.cling);
        }

    }


    protected override void ToolPickup(GameObject obtainer)
    {
        base.ToolPickup(obtainer);

        _obtainerController = obtainer.GetComponent<PersonController>();
        _obtainerStateMachine ??= _obtainerController.MovementSM;

        Debug.Log("Climbing Gauntlet got picked up by: " + obtainer.name);

    }

    protected override void ToolDrop(GameObject obtainer)
    {
        base.ToolDrop(obtainer);

        _obtainerController = null;
        _obtainerStateMachine = null;

        Debug.Log("Climbing Gauntlet got dropped by: " + obtainer.name);

    }
}
