using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.Team.Control
{
    public interface ITeamControl
    {
        void OpenControl();

        void CloseControl();
    }

    public enum CommandId
    {
        Move,
        Attack,
        /// <summary>
        /// 采集
        /// </summary>
        Gather,
    }

    public enum CommandType
    {
        AI,
        Keyboard,
        FrameSync,
    }



    public interface CanExecuteCommands
    {
        List<CommandId> CommandIds { get; }
    }
}
