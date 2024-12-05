using Assets.GameClientLib.Scripts.AI.ScountEnemy;
using Assets.Scripts.Common.Enum;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules.AI.Role
{
    [TaskCategory(BTShareStr.taskCategoryRole)]
    [TaskDescription("侦察敌人")]
    public class RoleScout : Action
    {
        public float radious;

        private bool hasEnemy;
        private IScoutEnemy scoutEnemy;
        private List<GameObject> enemyList;


        public override void OnAwake()
        {
            scoutEnemy = new ScoutEnemyByCircle(radious);
        }

        public override TaskStatus OnUpdate()
        {
            enemyList = scoutEnemy.GetNearEnemies();
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

    }
}
