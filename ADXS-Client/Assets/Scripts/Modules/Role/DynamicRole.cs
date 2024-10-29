using Assets.Scripts.Modules.RoleAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.Role
{
    public class DynamicRole : RoleBase
    {
        public DynamicRoleAttributes roleAttributes;

        public DynamicRole(DynamicRoleAttributes _attributes) : base()
        {
            attributesBase = roleAttributes = _attributes;
        }
    }
}
