using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.SteeringBehaviors;
using UnityEngine;

namespace Modules.AI.Move
{
    public class MoveAIByBoid : MoveAIBase
    {
        private MoveInfoByBoid info => (MoveInfoByBoid)moveInfo;


        public MoveAIByBoid(GameRoleCtrl role) : base(role)
        {
        }

        public override void OnMove(IMoveInfo _moveInfo)
        {
            moveInfo = _moveInfo;
            isFrame = true;

            info.IsArrive = IsArrive;
            if (!IsAlive)
            {
                moveTask = role.StartCoroutine(Move());
            }
        }

        private bool isFrame;

        private bool IsArrive()
        {
            if (isFrame)
            {
                isFrame = false;
                return false;
            }

            bool result = true;
            if (info.leader.Velocity.magnitude > 0.01f)
            {
                result = false;
                return result;
            }

            foreach (var item in info.boids)
            {
                if (item.Velocity.magnitude > 0.01f)
                {
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
            if (Host == info.leader)
            {
                steering += Host.SteeringBehavior.Arrive(info.Destination, info.arriveDistance);
                steering += Host.SteeringBehavior.Separation(info.boids, 5, info.separateDistance);
            }
            else
            {
                steering += Host.SteeringBehavior.BoidBehavior(info.leader, info.boids, info.groupNum, info.arriveDistance, info.separateDistance);
            }


            Host.SteeringBehavior.Update(steering);
            if (Host.Velocity.magnitude < ignoreValue && role.currentState == StateName.Move)
            {
                role.stateMachine.TryTrigger(StateName.Idle);
            }

            if (Host.Velocity.magnitude > ignoreValue && role.currentState != StateName.Move)
            {
                role.stateMachine.TryTrigger(StateName.Move);
            }

            if (Host.Velocity.magnitude > ignoreValue)
            {
                rb.velocity = Host.Velocity;
                var rotation = Quaternion.FromToRotation(Vector3.forward, Host.Velocity.normalized);
                if (rotation != transform.rotation)
                {
                    var rotationCoeff = 4f;
                    var ip = Mathf.Exp(-rotationCoeff * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(rotation, transform.rotation, ip);
                }
            }
        }
    }
}