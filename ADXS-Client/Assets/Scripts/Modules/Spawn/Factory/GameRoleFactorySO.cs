using Assets.Scripts.Modules.Role;
using UnityEngine;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.FSM;
using System.Collections.Generic;
using Assets.GameClientLib.Scripts.Utils.FSM;


namespace Assets.Scripts.Factory
{
    [CreateAssetMenu(fileName = "NewRoleFactory", menuName = "ScriptableObject/Factory/Role")]
    public class GameRoleFactorySO : GameUnitFactorySO<GameRoleCtrl, GameRole>
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

        protected override GameRole CreateEnity(GameRoleCtrl _ctrl)
        {
            GameRole role = base.CreateEnity(_ctrl);
            role.roleType = roleType;
            role.raceType = raceType;
            role.stateMachine = CreateFSM(_ctrl);
            role.roleAttributes = CreateAttributes();
            return role;
        }

        protected RoleAttributes CreateAttributes()
        {
            RoleAttributes model = new RoleAttributes();
            model.maxHP = maxHP;
            model.HPRestore = HPRestore;
            model.maxMP = maxMP;
            model.MPRestore = MPRestore;
            model.defense = defense;
            model.attack = attack;
            model.attackSpeed = attackSpeed;
            model.moveSpeed = moveSpeed;
            return model;
        }

        #region FSM

        protected virtual RoleStateMachine CreateFSM(GameRoleCtrl _ctrl)
        {
            List<State> states = new List<State>()
            {
                Idle(),
                Move(),
            };
            return new RoleStateMachine(states, RoleState.Idle, _ctrl);
        }

        protected virtual State Idle()
        {
            RoleStateBase idle = RoleStateBase.QuickCreate(RoleState.Idle, new List<RoleState> { RoleState.Move });
            return idle;
        }

        protected virtual State Move()
        {
            RoleStateBase idle = RoleStateBase.QuickCreate(RoleState.Move, new List<RoleState> { RoleState.Idle });
            return idle;
        }


        #endregion
    }
}
