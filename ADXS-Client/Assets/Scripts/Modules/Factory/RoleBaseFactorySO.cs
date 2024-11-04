using Assets.Scripts.Modules.Role;
using UnityEngine;
using System;
using Assets.Scripts.Common.Enum;
using System.Collections.Generic;


namespace Assets.Scripts.Factory
{
    [CreateAssetMenu(fileName = "RoleFactory", menuName = "ScriptableObject/Factory/Role")]
    public class RoleBaseFactorySO : GameUnitFactorySO<RoleBase>
    {
        public override RoleBase Create()
        {
            throw new NotImplementedException();
        }
    }
}
