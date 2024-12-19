using System;
using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Modules.FSM
{
    public class RoleTransition : Transition
    {
        public override string Id { get; }
        public Func<bool> canTransition { get; set; }
        public Action doTransition { get; set; }

        public RoleTransition(StateName _origin, StateName _next) : base(_origin.ToString(), _next.ToString())
        {
            Id = GenerateId(_origin, _next);
        }

        public static string GenerateId(StateName ori, StateName _next)
        {
            return ori.ToString() + "->" + _next.ToString();
        }

        public override bool CanTransition()
        {
            return canTransition == null ? true : canTransition();
        }

        public override void DoTransition()
        {
            doTransition?.Invoke();
        }
    }
}
