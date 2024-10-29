using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Buff;
using Assets.Scripts.Modules.Role.RoleState;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Modules.Role
{

    public class RoleBase
    {
        public string id { get; set; }
        public RoleType roleType { get; set; }
        public RoleAttributesBase attributesBase { get; set; }
        public RoleStateBase stateBase { get; set; }
        public List<BuffBase> buffsB { get; set; }

        public RoleBase()
        {
            id = Guid.NewGuid().ToString();
        }



    }
}