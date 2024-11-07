using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Buff;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Modules.Role
{
    [Serializable]
    public class RoleBase : GameUnit
    {
        public RoleType roleType;
        public Race raceType;
        public StateMachine stateMachine;
        public List<BuffBase> buffList;
        public BuffBase buff;
        public RoleAttributes roleAttributes;

        public RoleBase()
        {
            unitType = GameUnitType.Role;

        }
    }
}
