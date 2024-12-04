using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.Command
{
    public interface IMove
    {
        void Move(Vector3 targetPoint);
    }

    public interface IAttack
    {
        void Attack(GameRoleCtrl role);
    }

    public interface IIdle
    {
        void Idle();
    }
}
