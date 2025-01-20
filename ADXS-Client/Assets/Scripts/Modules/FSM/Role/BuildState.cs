using Assets.Scripts.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.FSM.Role
{
    internal class BuildState : RoleStateBase
    {
        public override StateName stateId => StateName.Build;

        protected override RoleAnimFlags animName => RoleAnimFlags.Build;

        protected float maxCoolingTime = 1f;
        protected float currentCoolingTime = 0;

        public override void OnEnter()
        {
            anim.SetTrigger(animNameHash);
        }

        public override void OnExit()
        {
            Debug.Log("exit");
        }

        public override void OnUpdate()
        {
            if (role.stateMachine.IsCurrentStateOnAnimation(StateName.Idle))
            {
                if (currentCoolingTime >= maxCoolingTime)
                {
                    //  anim.SetBool(StateName.Idle.ToString(), false);
                    anim.SetTrigger(animNameHash);
                    currentCoolingTime = 0;
                    Debug.Log(" anim.SetTrigger(animNameHash)");
                }
                currentCoolingTime += Time.deltaTime;
                Debug.Log("currentCoolingTime:" + currentCoolingTime);
            }
            else
            {
                var arr = role.animator.GetCurrentAnimatorClipInfo(0);
                if (arr != null && arr.Length >= 1)
                {
                    string animString = arr[0].clip.name;
                    Debug.Log(animString);
                }
            }
        }

        protected override StateName[] InitNextState() => new StateName[] { StateName.Idle };

    }
}
