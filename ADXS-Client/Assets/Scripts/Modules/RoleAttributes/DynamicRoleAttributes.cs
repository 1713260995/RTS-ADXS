using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.RoleAttributes
{
    public class DynamicRoleAttributes : RoleAttributesBase
    {
        public float mp { get; set; }
        public float maxMp { get; set; }
        public float mpRestore { get; set; }
        public float attackSpeed { get; set; }
        public float attack { get; set; }
        public float moveSpeed { get; set; }
    }
}
