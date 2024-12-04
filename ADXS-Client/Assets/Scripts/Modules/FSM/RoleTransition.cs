using System;
using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Modules.FSM
{
    public class RoleTransition : Transition
    {
        private string _id;
        public override string Id => _id;
        public Func<bool> canTransition { get; set; }
        public Func<UniTask> doTransition { get; set; }

        public RoleTransition(RoleState _origin, RoleState _target) : base(_origin.ToString(), _target.ToString())
        {
            _id = GenerateId(_origin, _target);
        }

        public static string GenerateId(RoleState ori, RoleState target)
        {
            return ori.ToString() + "->" + target.ToString();
        }

        public override bool CanTransition()
        {
            return canTransition == null ? true : canTransition();
        }

        public override async UniTask DoTransition()
        {
            await (doTransition == null ? UniTask.CompletedTask : doTransition());
        }
    }
}
