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
        public RoleType roleType { get; set; }
        public Race raceType { get; set; }
        public StateMachine stateMachine { get; set; }
        public List<BuffBase> buffList { get; set; }
        public BuffBase buff { get; set; }
        public RoleAttributes roleAttributes { get; set; }

        public RoleBase()
        {
            unitType = GameUnitType.Role;
        }
    }
}
