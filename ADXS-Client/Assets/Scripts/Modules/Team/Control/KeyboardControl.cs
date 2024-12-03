using Assets.GameClientLib.Scripts;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Common.Enum;

namespace Assets.Scripts.Modules.Team.Control
{
    public class KeyboardCommand : ITeamControl
    {
        private InputHandler handler { get; set; }
        private Team team { get; set; }
        private List<GameUnitCtrl> selectedUnits { get; set; }

        public KeyboardCommand(Team _team)
        {
            team = _team;
            handler = InputHandler.Create();
            selectedUnits = new List<GameUnitCtrl>();
        }

        public void OpenControl()
        {
            handler.EnableMultipleSelect(MouseInfo.MouseId.Left, SelectableUnits, MultipleSelectCallback);
            handler.EnableSingleSelect(MouseInfo.MouseId.Left, SingleSelectCallback);
            handler.mouseRight.keyUpEvent += RightClickEvent;
        }

        public void CloseControl()
        {
            handler.DisableMultipleSelect();
            handler.DisableSingleSelect();
            handler.mouseRight.keyUpEvent -= RightClickEvent;
        }

        #region 选中单位
        /// <summary>
        /// 多选时可选单位
        /// </summary>
        public List<Component> SelectableUnits()
        {
            return GameUnitManager.Instance.allGameUnits.Cast<Component>().ToList();
        }

        /// <summary>
        /// 多选游戏单位回调
        /// </summary>
        /// <param name="list"></param>
        private void MultipleSelectCallback(List<Component> list)
        {
            selectedUnits = list.Cast<GameUnitCtrl>().ToList();
        }

        /// <summary>
        /// 单选游戏回调
        /// </summary>
        /// <param name="o"></param>
        private void SingleSelectCallback(Component o)
        {
            selectedUnits.Clear();
            if (o == null)
            {
                return;
            }
            GameUnitCtrl ctrl = o.GetComponent<GameUnitCtrl>();
            if (ctrl != null)
            {
                selectedUnits.Add(ctrl);
            }
        }

        public void PrintSelectUnits(string str)
        {
            Debug.Log("执行" + str);
            foreach (var item in selectedUnits)
            {
                Debug.Log("选中单位:" + item.name);
            }
        }
        #endregion

        #region 控制选中选中的单位


        public void RightClickEvent()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (selectedUnits.Count == 0)
                return;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer(GameLayer.Ground.ToString()))
                {
                    selectedUnits.First().transform.position = hit.point;
                    return;
                }
                GameUnitCtrl ctrl = hit.transform.GetComponent<GameUnitCtrl>();
                if (ctrl != null)
                {
                    Debug.Log("右击选中单位" + ctrl.name);
                }
                else
                {
                    Debug.Log("右击选中普通对象" + hit.transform.name);
                }
            }
        }
        #endregion
    }
}
