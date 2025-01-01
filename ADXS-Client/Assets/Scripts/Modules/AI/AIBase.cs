using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public abstract class AIBase : IAIBase
    {
        public GameRoleCtrl role { get; private set; }
        public Transform transform { get; private set; }
        public abstract bool IsAlive { get; }



        public AIBase(GameRoleCtrl role)
        {
            this.role = role;
            this.transform = role.transform;
            role.aIBases.Add(this);
        }



        public abstract void AbortAI();


    }
}
