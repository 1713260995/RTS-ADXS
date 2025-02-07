using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.SteeringBehaviors;
using UnityEngine;

namespace Modules.AI.Move
{
    public class MoveAIByBoid : MoveAIBase
    {
        protected IBoid host;
        private MoveInfoByBoid info => (MoveInfoByBoid)moveInfo;

        public MoveAIByBoid(GameRoleCtrl role) : base(role)
        {
            host = role.GetComponent<Boid>();
            if (host == null) {
                host = role.gameObject.AddComponent<Boid>();
            }
        }

        public override void OnMove(IMoveInfo _moveInfo)
        {
            moveInfo = _moveInfo;
            isFrame = true;

            info.IsArrive = IsArrive;
            if (!IsAlive) {
                moveTask = role.StartCoroutine(Move());
            }
        }

        private bool isFrame;

        private bool IsArrive()
        {
            if (isFrame) {
                isFrame = false;
                return false;
            }

            bool result = true;
            if (info.leader.Velocity.magnitude > 0.01f) {
                result = false;
                return result;
            }

            foreach (var item in info.boids) {
                if (item.Velocity.magnitude > 0.01f) {
                    result = false;
                    break;
                }
            }

            return result;
        }

        protected override void UpdatePosAndDir()
        {
            Vector3 steering = Vector3.zero;
            float ignoreValue = 0.04f;
            if (host == info.leader) {
                steering += host.SteeringBehavior.Arrive(info.Destination, info.arriveDistance);
                steering += host.SteeringBehavior.CollisionAvoidance(10);
            }
            else {
                steering += host.SteeringBehavior.BoidBehavior((Boid)info.leader, info.boids, info.groupNum, info.arriveDistance, info.separateDistance);
            }

            host.SteeringBehavior.Update(steering);
            if (host.Velocity.magnitude < ignoreValue && role.currentState == StateName.Move) {
                role.stateMachine.TryTrigger(StateName.Idle);
            }

            if (host.Velocity.magnitude > ignoreValue && role.currentState != StateName.Move) {
                role.stateMachine.TryTrigger(StateName.Move);
            }

            if (host.Velocity.magnitude > ignoreValue) {
                transform.position += host.Velocity * Time.deltaTime;
                var rotation = Quaternion.FromToRotation(Vector3.forward, host.Velocity.normalized);
                if (rotation != transform.rotation) {
                    var rotationCoeff = 4f;
                    var ip = Mathf.Exp(-rotationCoeff * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(rotation, transform.rotation, ip);
                }
            }
        }
    }
}