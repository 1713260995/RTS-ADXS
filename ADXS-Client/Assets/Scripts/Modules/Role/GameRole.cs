using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Buff;
using Assets.Scripts.Modules.FSM;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Modules.Role
{
    [Serializable]
    public class GameRole : GameUnit
    {
        public RoleType roleType;
        public Race raceType;
        public RoleStateMachine stateMachine;
        public List<BuffBase> buffList;
        public BuffBase buff;
        public RoleAttributes roleAttributes;
        public List<CommandId> commandIds;
    }
}
