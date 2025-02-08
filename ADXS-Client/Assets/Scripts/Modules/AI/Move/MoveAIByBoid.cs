using System.Collections;
using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.SteeringBehaviors;
using UnityEngine;

namespace Modules.AI.Move
{
    public class MoveAIByBoid : MoveAIBase
    {
        private SteeringBehavior.BoidBehaviorInfo info;

        public MoveAIByBoid(GameRoleCtrl role) : base(role)
        { }

        public override void OnMove(IMoveInfo _moveInfo)
        {
            moveInfo = _moveInfo;
            info = ((MoveInfoByBoid)moveInfo).info;
            if (!IsAlive) {
                moveTask = role.StartCoroutine(Move());
            }
        }

        protected override IEnumerator Move()
        {
            UpdatePosAndDir();
            return base.Move();
        }

        protected override void UpdatePosAndDir()
        {
            Vector2 steering = Vector2.zero;
            float ignoreValue = 1f;
            steering += Host.SteeringBehavior.BoidBehavior(info);
            Host.SteeringBehavior.Update(steering);
            if (Host.Velocity.magnitude < ignoreValue && role.currentState == StateName.Move) {
                role.stateMachine.TryTrigger(StateName.Idle);
            }

            if (Host.Velocity.magnitude > ignoreValue && role.currentState != StateName.Move) {
                role.stateMachine.TryTrigger(StateName.Move);
            }

            if (Host.Velocity.magnitude > ignoreValue) {
                rb.velocity = Host.Velocity.XZToXYZ();
                var rotation = Quaternion.FromToRotation(Vector3.forward, Host.Velocity.XZToXYZ().normalized);
                if (rotation != transform.rotation) {
                    var rotationCoeff = 4f;
                    var ip = Mathf.Exp(-rotationCoeff * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(rotation, transform.rotation, ip);
                }
            }
        }
    }
}