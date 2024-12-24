using Assets.Scripts.Game;
using Assets.Scripts.Modules.Battle;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
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
        }

        public Vector3 currentEndPoint;

        public bool FindWay(Vector3 point, Action onComplete)
        {
            bool isSuccess = agent.SetDestination(point);
            if (!isSuccess) return false;
            currentEndPoint = point;
            BattleSystem.Instance.StartCoroutine(WaitComplete(point, onComplete));
            return true;
        }


        public IEnumerator WaitComplete(Vector3 endPoint, Action onComplete)
        {
            while (true)
            {
                if (endPoint != currentEndPoint)
                {
                    yield break;
                }
                if ((endPoint - agent.transform.position).magnitude <= 0.5f)
                {
                    if (agent.pathStatus == NavMeshPathStatus.PathComplete)
                    {
                        onComplete.Invoke();
                        Debug.Log("到达终点");
                        yield break;
                    }
                }
                yield return null;
            }
        }
    }
}
