using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Modules.AI.FindWay
{
    public class FindWayByNav : IFindWay
    {
        private NavMeshAgent agent;
        private Action onComplete;
        private Vector3? endPoint;

        public FindWayByNav(GameRoleCtrl _ctrl)
        {
            agent = _ctrl.GetComponent<NavMeshAgent>();
            Wait().Forget();
        }


        public bool FindWay(Vector3 point, Action onComplete)
        {
            bool isSuccess = agent.SetDestination(point);
            if (!isSuccess) return false;
            this.onComplete = onComplete;
            endPoint = point;
            return true;
        }


        private async UniTask Wait()
        {
            while (true)
            {
                if (agent == null)
                {
                    return;
                }
                if (endPoint != null && onComplete != null && (endPoint.Value - agent.transform.position).magnitude <= 1f)
                {
                    Debug.Log("到达终点");
                    onComplete.Invoke();
                    onComplete = null;
                    endPoint = null;
                }
                await UniTask.Yield();
            }
        }

    }
}
