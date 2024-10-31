using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Buff;
using System.Collections.Generic;

namespace Assets.Scripts.Modules.Role
{
    public class RoleBase : GameUnit
    {
        public Race raceType { get; set; }
        public StateMachine stateBase { get; set; }
        public List<BuffBase> buffList { get; set; }
        public RoleAttributes roleAttributes { get; set; }

        public RoleBase() : base()
        {

        }
    }
}
