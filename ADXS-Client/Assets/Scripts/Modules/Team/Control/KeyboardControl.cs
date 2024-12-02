using Assets.GameClientLib.Scripts;
using Assets.Scripts.Modules.Role;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.GameClientLib.Resource;
using System.Linq;

namespace Assets.Scripts.Modules.Team.Control
{
    public class KeyboardCommand : ITeamControl
    {
        public GameRole role;
        public InputHandler handler;
        public Action<List<Transform>> selectObjsCallBack;
        public List<Transform> teamUnitList { get; set; }

        public KeyboardCommand(List<Transform> _teamUnitList, Action<List<Transform>> _selectObjsCallBack)
        {
            teamUnitList = _teamUnitList;
            selectObjsCallBack = _selectObjsCallBack;
            handler = InputHandler.Create();
        }

        public void OpenControl()
        {

            handler.EnableMultipleSelect(MouseInfo.MouseId.Left, teamUnitList, selectObjsCallBack);
            handler.EnableSingleSelect(MouseInfo.MouseId.Left, IsSelectRole, SingleRoleSuccess);
        }

        public void CloseControl()
        {
            handler.DisableMultipleSelect();
            handler.DisableSingleSelect();
        }

        public bool IsSelectRole(Transform o)
        {
            if (o.GetComponent<GameUnitCtrl>() == null)
            {
                return false;
            }
            return true;
        }

        public void SingleRoleSuccess(Transform o)
        {
            Debug.Log("SingleRoleSuccess:" + o.name);
        }










    }
}
