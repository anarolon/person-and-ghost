using UnityEngine;

namespace PersonAndGhost.Person.States
{
    public class GrappleAimState : PersonState
    {
        private Vector2 _movementInput;
        private bool _jumped;

        [Header("Grappling Fields")]
        private float _aimSpeed = 3.0f;
        private GameObject _grappleFiringPoint;
        private LineRenderer _grappleLine;

        public GrappleAimState(PersonMovement character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.GrapplePoint = Vector2.one * 10; // TEMP VALUE
            SetupGrappleLine();
            
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
            if (character.DidReachGrapplePoint())
            {
                //Debug.Log("Reached the destination");
                if (character.IsOnGround) stateMachine.ChangeState(character.idle);
                else stateMachine.ChangeState(character.falling);
            }


        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            _grappleLine.SetPosition(0, character.transform.position);
            if (_jumped)
            {
                character.SetGrapplePoint(_grappleFiringPoint.transform.up);
                if (character.CanGrapple && !character.DidReachGrapplePoint())
                {
                    _grappleLine.enabled = true;
                    _grappleLine.SetPosition(1, character.GrapplePoint);
                    character.Grapple();
                }
                else
                {
                    //Debug.Log("Cancelled Grappling");
                    if (character.IsOnGround) stateMachine.ChangeState(character.idle);
                    else stateMachine.ChangeState(character.falling);
                }
                
            }
            else if(_movementInput != Vector2.zero)
            {
                _grappleFiringPoint.transform.Rotate(0, 0, -_movementInput.x * Time.deltaTime * _aimSpeed * Mathf.Rad2Deg); 
               
            }
        }

        public override void Exit()
        {
            base.Exit();
            character.ResetVelocity();
            DestroyGrappleLine();
        }

        private void SetupGrappleLine()
        {
            _grappleFiringPoint ??= GameObject.Instantiate(Resources.Load("Prefabs/GrappleAim")) as GameObject;
            _grappleFiringPoint.transform.SetParent(character.transform);
            _grappleFiringPoint.transform.localPosition = Vector2.zero;

            _grappleLine = _grappleFiringPoint.GetComponent<LineRenderer>();
            _grappleLine.startWidth = 0.2f;
            _grappleLine.endWidth = 0.2f;
            _grappleLine.positionCount = 2;
            _grappleLine.enabled = false;
        }

        private void DestroyGrappleLine()
        {
            //_grappleLine = null;
            GameObject.Destroy(_grappleLine);
            GameObject.Destroy(_grappleFiringPoint);
            _grappleFiringPoint = null;
        }

        public override string StateId() {
            return "GrappleAimState";
        }
    }
}
