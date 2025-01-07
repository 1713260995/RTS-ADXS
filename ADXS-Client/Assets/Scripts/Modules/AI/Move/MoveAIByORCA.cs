using Assets.GameClientLib.Scripts.Utils;
using GameClientLib.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public class MoveAIByORCA : MoveAIBase
    {
        public ORCAAgentInfo orcaAgentInfo { get; private set; }

        /// <summary>
        /// orca无解时，停止移动，开始休息
        /// </summary>
        private float unsolvableStopDis = 0.15f;

        public MoveAIByORCA(GameRoleCtrl role) : base(role)
        {
            orcaAgentInfo = new ORCAAgentInfo(role.transform, maxSpeed: role.MoveSpeed);
            ORCASystem.Instance.AddAgent(orcaAgentInfo, Vector2.zero);
            orcaAgentInfo.SetGoalPoint(role.transform.position);
            role.OnDead += orcaAgentInfo.Remove;
        }

        public override void OnMove(MoveInfo _moveInfo)
        {
            base.OnMove(_moveInfo);
            orcaAgentInfo.SetGoalPoint(_moveInfo.endPoint);
        }

        protected override void UpdatePosAndDir()
        {
            Vector3 velocity = orcaAgentInfo.GetAgentCurrentVelocity();
            if (role.currentState == StateName.Move)
            {
                Vector3 rotateEuler = MyMath.LookAt(transform, orcaAgentInfo.agentNewPosition);
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, rotateEuler, MyMath.GetLerp(role.rotateLerp));
            }

            if (velocity.magnitude <= unsolvableStopDis && role.currentState == StateName.Move)
            {
                role.stateMachine.TryTrigger(StateName.Idle);
            }

            if (velocity.magnitude > unsolvableStopDis && role.currentState == StateName.Idle)
            {
                role.stateMachine.TryTrigger(StateName.Move);
            }

            transform.position = orcaAgentInfo.agentNewPosition;
        }
    }
}