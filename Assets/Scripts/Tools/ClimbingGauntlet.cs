using UnityEngine;

public class ClimbingGauntlet : Tool
{
    private PersonController obtainerController = default;
    private StateMachine obtainerStateMachine = default;

    protected override void ToolAction(GameObject obtainer)
    {
        base.ToolAction(obtainer);

        if (obtainerController != null && obtainerStateMachine != null
            && obtainerController.IsTouchingWall)
        {
            obtainerStateMachine.ChangeState(obtainerController.cling);
        }

    }


    protected override void ToolPickup(GameObject obtainer)
    {
        base.ToolPickup(obtainer);

        obtainerController = obtainer.GetComponent<PersonController>();
        obtainerStateMachine ??= obtainerController.MovementSM;

        Debug.Log("Climbing Gauntlet got picked up by: " + obtainer.name);

    }

    protected override void ToolDrop(GameObject obtainer)
    {
        base.ToolDrop(obtainer);

        obtainerController = null;
        obtainerStateMachine = null;

        Debug.Log("Climbing Gauntlet got dropped by: " + obtainer.name);

    }
}
