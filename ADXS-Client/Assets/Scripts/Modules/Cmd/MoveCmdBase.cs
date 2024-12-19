using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.Cmd
{
    public abstract class MoveCmdBase : CmdBase
    {
        protected override StateName stateId => StateName.Move;

        public MoveCmdBase(GameRoleCtrl _ctrl) : base(_ctrl)
        {
        }

        public override bool Execute<T>(T obj)
        {
            if (!(obj is Vector3 point))
            {
                throw new ArgumentException("args is error");
            }
            return Move(point);
        }

        public abstract bool Move(Vector3 point);
    }
}
