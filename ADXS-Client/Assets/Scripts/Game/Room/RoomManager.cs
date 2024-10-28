using Assets.GameClientLib.Utils.Singleton;
using Assets.Scripts.Entity;
using Assets.Scripts.Modules.Battle;

namespace Assets.Scripts.Modules
{
    /// <summary>
    /// 1.房主创建房间，并设置房间信息（最大人数，机器人数量，机器人阵营）
    /// 2.其他玩家查看房间列表
    /// 3.其他玩家进入想进的房间
    /// 4.所有玩家选择阵营
    /// 5.所有玩家点击准备完成
    /// 6.房主点击开始游戏
    /// 7.所有玩家开始加载战斗场景，并在加载完成后通知服务器
    /// 8.等待所有玩家加载完成，服务器通知所有玩家跳转至战斗场景
    /// </summary>
    public class RoomManager : Singleton<RoomManager>
    {
        private ConnectUDPServer connectServer;

        public RoomManager()
        {
            connectServer = new ConnectUDPServer();
        }

        public void ConnectServer()
        {
            connectServer.Send();
        }

        public void CreateRoom()
        {
            Room room = new Room();
        }
    }
}
