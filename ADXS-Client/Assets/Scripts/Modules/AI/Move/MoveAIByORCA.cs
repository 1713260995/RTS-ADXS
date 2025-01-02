using GameClientLib.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public class MoveAIByORCA : MoveAIBase
    {
        public ORCAAgentInfo orcaAgentInfo { get; private set; }

        public MoveAIByORCA(GameRoleCtrl role) : base(role) {
            orcaAgentInfo = new ORCAAgentInfo(role.transform, currentVelocity: Vector3.zero, maxSpeed: role.MoveSpeed);
            ORCASystem.Instance.AddAgent(orcaAgentInfo);
            orcaAgentInfo.SetGoalPoint(role.transform.position);
        }

        public override void OnMove(MoveInfo _moveInfo) {
            base.OnMove(_moveInfo);
            orcaAgentInfo.SetGoalPoint(_moveInfo.endPoint);
        }

        protected override void UpdatePos() {
            transform.position = orcaAgentInfo.agentNewPosition;
        }
    }
}