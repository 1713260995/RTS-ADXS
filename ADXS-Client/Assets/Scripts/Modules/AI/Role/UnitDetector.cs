using Assets.GameClientLib.Scripts.AI;
using Assets.Scripts.Common.Enum;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.AI.Role
{
    [TaskCategory(BTShareStr.taskCategoryRole)]
    [TaskDescription("侦察敌人")]
    public class UnitDetector : Action
    {
        public float disRange;

        private bool hasEnemy;
        private IUnitDetector<GameRoleCtrl> detector;
        private List<GameRoleCtrl> enemyList;
        private GameRoleCtrl roleCtrl;


        public override void OnAwake()
        {
            roleCtrl = GetComponent<GameRoleCtrl>();
            detector = new UnitDetectorByPhysicsSphere<GameRoleCtrl>(roleCtrl, disRange, GameLayerName.Role.GetLayer());
        }

        public override TaskStatus OnUpdate()
        {
            enemyList = detector.GetNearbyObjs();
            enemyList = enemyList.Where(o => o.agent.id != roleCtrl.agent.id).ToList();
            if (enemyList.Count == 0)
            {
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }

        public override void OnEnd()
        {
            Owner.SendEvent("ChangeRoleState", RoleState.Attack);
        }

        public override void OnDrawGizmos()
        {
            detector.OnDrawRange();
        }

    }
}
