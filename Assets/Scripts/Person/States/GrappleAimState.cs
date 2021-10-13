using UnityEngine;

namespace PersonAndGhost.Person.States
{
    public class GrappleAimState : PersonState
    {
        private Vector2 _movementInput;
        private bool _jumped;

        [Header("Grappling Fields")]
        private float _aimSpeed = 2.0f;
        private GameObject _grappleFiringPoint;

        public GrappleAimState(PersonMovement character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _grappleFiringPoint = new GameObject("Grapple Aim");
            _grappleFiringPoint.transform.SetParent(character.transform);
            _grappleFiringPoint.transform.localPosition = Vector2.zero;
            
        }

        public override void HandleInput()
        {
            base.HandleInput();
            _movementInput = character.MovementInput;
            _jumped = character.Jumped;

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // TODO: Switch to falling or idle on cancel button input
            if (character.DidReachGrapplePoint())
            {
                Debug.Log("Reached the destination");
                if (character.IsOnGround) stateMachine.ChangeState(character.idle);
                else stateMachine.ChangeState(character.falling);
            }

        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            // TODO: Shoot grapple hook on jump input
            if (_jumped)
            {
                character.GrappleTowardsPoint(_grappleFiringPoint.transform.right);
            }
            // TODO: Move gun direction if movement input
            else if(_movementInput != Vector2.zero)
            {
                _grappleFiringPoint.transform.Rotate(0, 0, _movementInput.x * Time.deltaTime * _aimSpeed * Mathf.Rad2Deg);
            }
           

        }

        public override void Exit()
        {
            base.Exit();
            character.ResetVelocity();
            GameObject.Destroy(_grappleFiringPoint);
            _grappleFiringPoint = null;
        }
    }
}
