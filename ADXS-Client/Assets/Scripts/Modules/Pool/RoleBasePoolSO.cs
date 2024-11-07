using Assets.Scripts.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.Pool
{
    [CreateAssetMenu(fileName = "NewRolePool", menuName = "ScriptableObject/Pool/Role Rool")]
    public class RoleBasePoolSO : GameUnitPoolSO<RoleBaseCtrl>
    {


    }
}
