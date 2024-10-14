using Assets.Scripts.Modules.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.Battle
{
    public class BattleSystem
    {
        private ConnectUDPServer connectServer;

        public BattleSystem()
        {
            connectServer = new ConnectUDPServer();
        }

        public void ConnectServer()
        {
            connectServer.Send();
        }
    }
}
