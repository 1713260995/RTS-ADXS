using Assets.GameClientLib.Scripts;
using Assets.Scripts.Modules.Role;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules
{

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

    public interface ITeamControl
    {
        void EnableControl();
        void DisableControl();
    }

    public interface CanExecuteCommands
    {
        List<CommandId> CommandIds { get; }
    }


    public class KeyboardCommand : ITeamControl
    {
        public GameRole role;
        public InputHandler handler;

        public void EnableControl()
        {
            //  handler.EnableDrawRect();
        }

        public void DisableControl()
        {
            throw new System.NotImplementedException();
        }


    }
}
