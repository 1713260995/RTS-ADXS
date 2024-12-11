using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Modules.Spawn;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    public class Agent
    {

        public int id { get; private set; }
        public int teamId { get; private set; }
        public GameColor color { get; private set; }
        private IAgentControl control { get; set; }

        public List<GameUnitCtrl> gameUnitCtrls { get; set; }


        public Agent(int _teamId, GameColor _color, AgentControlWay controlWay)
        {
            id = MyMath.UniqueNum();
            teamId = _teamId;
            color = _color;
            GenerateControl(controlWay);
        }

        public void OnEnable()
        {
            control.OpenControl();
        }

        public void OnDisable()
        {
            control.CloseControl();
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
    }
}
