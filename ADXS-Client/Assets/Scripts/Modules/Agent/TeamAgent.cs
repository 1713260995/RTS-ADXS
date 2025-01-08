using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Modules.Spawn;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    /// <summary>
    /// 每个团队由一个Agent进行控制
    /// </summary>
    public class TeamAgent
    {
        public int id { get; private set; }
        /// <summary>
        /// 多个Agent可以属于同一个组
        /// </summary>
        public int groupId { get; private set; }
        public GameColor color { get; private set; }
        private IAgentControl control { get; set; }
        public List<GameUnitCtrl> allUnits { get; set; }

        public TeamAgent(int _groupId, GameColor _color, AgentControlWay controlWay)
        {
            id = MyMath.UniqueNum();
            groupId = _groupId;
            color = _color;
            allUnits = new();
            GenerateControl(controlWay);
        }

        public void OnEnable()
        {
            control?.OpenControl();
        }

        public void OnDisable()
        {
            control?.CloseControl();
        }

        private void GenerateControl(AgentControlWay controlWay)
        {
            switch (controlWay)
            {
                case AgentControlWay.AI:
                    break;
                case AgentControlWay.Keyboard:
                    control = new KeyboardCommand(this);
                    break;
                case AgentControlWay.FrameSync:
                    break;
            }
        }

        public override string ToString()
        {
            return $"id={id},teamId={groupId},color={color},control={control}";
        }

    }
}
