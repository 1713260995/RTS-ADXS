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
        public RoleAttributesBase attributes { get; set; }
        public RoleStateBase state { get; set; }
        public List<BuffBase> buffs { get; set; }

        public RoleBase()
        {
            id = Guid.NewGuid().ToString();
        }

        public void CreateRole()
        {

        }

    }
}