using Assets.GameClientLib.Scripts;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Common.Enum;

namespace Assets.Scripts.Modules
{
    public class KeyboardCommand : IAgentControl
    {
        public TeamAgent agent { get; }
        private InputHandler handler { get; set; }
        private Army currentArmy { get; set; }
        private KeyCode idleKey = KeyCode.S;
        private KeyCode cameraFollowKey = KeyCode.Space;
        private List<GameUnitCtrl> selectUnits = new List<GameUnitCtrl>();
        private bool isRunning;

        public KeyboardCommand(TeamAgent _agent)
        {
            agent = _agent;
            handler = InputHandler.Create();
            currentArmy = new Army(agent.id);
        }

        #region Control

        public void OpenControl()
        {
            if (isRunning) return;
            isRunning = true;
            handler.EnableSingleSelect(MouseInfo.MouseId.Left, SingleSelectCallback);
            handler.EnableMultipleSelect(MouseInfo.MouseId.Left, MultipleSelectAbleUnits, MultipleSelectCallback);
            handler.mouseRight.keyUpEvent += RightClickEvent;
            handler.AddkeyboardDownEvent(idleKey, Idle);
            handler.AddkeyboardStayEvent(cameraFollowKey, CameraFollow);
        }

        public void CloseControl()
        {
            if (!isRunning) return;
            isRunning = false;
            handler.DisableSingleSelect();
            handler.DisableMultipleSelect();
            handler.mouseRight.keyUpEvent -= RightClickEvent;
            handler.RemovekeyboardDownEvent(idleKey, Idle);
            handler.RemovekeyboardStayEvent(cameraFollowKey, CameraFollow);
        }

        #endregion

        #region Select Units

        /// <summary>
        /// 多选时可选单位
        /// 1.多选时只可以选我方单位
        /// </summary>
        public List<Component> MultipleSelectAbleUnits()
        {
            return agent.allUnits.Where(o => o is GameRoleCtrl).Cast<Component>().ToList();
        }

        /// <summary>
        /// 多选游戏单位回调
        /// </summary>
        /// <param name="list"></param>
        private void MultipleSelectCallback(List<Component> list)
        {
            selectUnits = list.Cast<GameUnitCtrl>().ToList();
            currentArmy.ReplaceMembers(list.Cast<GameRoleCtrl>().ToList());
        }

        /// <summary>
        /// 单选回调。
        /// 1. 单选没有可选单位，所以可以是任何单位或者为null
        /// </summary>
        /// <param name="o"></param>
        private void SingleSelectCallback(Component o)
        {
            GameUnitCtrl ctrl = o == null ? null : o.GetComponent<GameUnitCtrl>();
            if (ctrl == null)
            {
                selectUnits.Clear();
                currentArmy.ReplaceMember(null);
            }
            else
            {
                selectUnits.Add(ctrl);
                if (ctrl is GameRoleCtrl role && role.agent.id == agent.id)
                {
                    currentArmy.ReplaceMember(role);
                }
                else
                {
                    currentArmy.ReplaceMember(null);
                }
            }
        }

        #endregion

        #region Control the selected units

        private int waterLayerMask = GameLayerName.Water.GetLayerMask();
        private int sceneLayerMask = GameLayerName.Scene.GetLayerMask();
        private int groundLayerMask = GameLayerName.Ground.GetLayerMask();

        public void RightClickEvent()
        {
            Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                int hitLayerMask = 1 << hit.transform.gameObject.layer;
                if ((hitLayerMask & (waterLayerMask | sceneLayerMask)) != 0)
                {
                    Debug.Log("点击到水或场景，无法操作");
                    return;
                }
                if (hitLayerMask == groundLayerMask)
                {
                    currentArmy.Move(hit.point);
                    return;
                }
                GameUnitCtrl ctrl = hit.transform.GetComponent<GameUnitCtrl>();
                if (ctrl == null)
                {
                    return;
                }
                currentArmy.Attack(ctrl);
            }
        }

        public void Idle()
        {
            currentArmy.Idle();
        }

        #endregion

        #region Camera Follow

        public Transform cameraTran;

        public void CameraFollow()
        {
            var ctrl = GetUnitFirstOrDefault();
            if (ctrl != null)
            {
                Vector3 pos = ctrl.transform.position;
                if (cameraTran == null)
                {
                    cameraTran = Camera.main!.transform;
                }
                cameraTran.position = new Vector3(pos.x, cameraTran.transform.position.y, pos.z - 5);
            }
        }
        #endregion

        #region Helper

        private GameUnitCtrl GetUnitFirstOrDefault()
        {
            //TODO 后面可能需要修改：第一个单位是英雄
            return selectUnits?.FirstOrDefault();
        }

        #endregion
    }
}
