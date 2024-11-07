using Assets.Scripts.Modules.Role;
using UnityEngine;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;


namespace Assets.Scripts.Factory
{
    [CreateAssetMenu(fileName = "NewRoleFactory", menuName = "ScriptableObject/Factory/Role")]
    public class RoleBaseFactorySO : GameUnitFactorySO<RoleBaseCtrl, RoleBase>
    {
        public RoleType roleType;
        public Race raceType;

        public float maxHP;
        public float HPRestore;//生命恢复
        public float maxMP;
        public float MPRestore;//魔法恢复
        public float defense;
        public float attack;
        public float attackSpeed;
        public float moveSpeed;


        protected override RoleBase CreateEnity()
        {
            RoleBase role = base.CreateEnity();
            role.roleType = roleType;
            role.raceType = raceType;
            role.roleAttributes = CreateAttributes();
            return role;
        }

        protected RoleAttributes CreateAttributes()
        {
            RoleAttributes model = new RoleAttributes();
            model.maxMP = maxMP;
            model.HPRestore = HPRestore;
            model.maxMP = maxMP;
            model.MPRestore = MPRestore;
            model.defense = defense;
            model.attack = attack;
            model.attackSpeed = attackSpeed;
            model.moveSpeed = moveSpeed;
            return model;
        }
    }
}
