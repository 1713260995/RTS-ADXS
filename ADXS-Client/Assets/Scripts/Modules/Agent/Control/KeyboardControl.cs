using Assets.GameClientLib.Scripts;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Common.Enum;

namespace Assets.Scripts.Modules
{
    public class KeyboardCommand : IAgentControl
    {
        public Agent agent { get; set; }

        private InputHandler handler { get; set; }
        private List<GameUnitCtrl> selectedUnits { get; set; }



        public KeyboardCommand()
        {
            handler = InputHandler.Create();
            selectedUnits = new List<GameUnitCtrl>();
        }

        public void OpenControl()
        {
            handler.EnableSingleSelect(MouseInfo.MouseId.Left, SingleSelectCallback);
            handler.EnableMultipleSelect(MouseInfo.MouseId.Left, SelectableUnits, MultipleSelectCallback);
            handler.mouseRight.keyUpEvent += RightClickEvent;
        }

        public void CloseControl()
        {
            handler.DisableSingleSelect();
            handler.DisableMultipleSelect();
            handler.mouseRight.keyUpEvent -= RightClickEvent;
        }

        #region 选中单位

        /// <summary>
        /// 多选时可选单位
        /// 1.多选时只可以选我方单位
        /// </summary>
        public List<Component> SelectableUnits()
        {
            return agent.gameUnitCtrls.Cast<Component>().ToList();
        }

        /// <summary>
        /// 多选游戏单位回调
        /// </summary>
        /// <param name="list"></param>
        private void MultipleSelectCallback(List<Component> list)
        {
            selectedUnits.Clear();
            selectedUnits.AddRange(list.Cast<GameUnitCtrl>().ToList());
        }

        /// <summary>
        /// 单选游戏回调
        /// </summary>
        /// <param name="o"></param>
        private void SingleSelectCallback(Component o)
        {
            selectedUnits.Clear();
            if (o == null)
                return;//单击未选中时直接返回，o==null
            GameUnitCtrl ctrl = o.GetComponent<GameUnitCtrl>();
            if (ctrl == null)
                return;//单击选中有碰撞体的物体时，但不属于游戏单位时返回。
            selectedUnits.Add(ctrl);
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
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer(GameLayerName.Ground.ToString()))
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
