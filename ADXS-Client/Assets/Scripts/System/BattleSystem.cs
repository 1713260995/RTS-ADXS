using Assets.Scripts.Modules.Spawn;
using System.Collections.Generic;
using System.Linq;
using GameClientLib.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Modules.Battle
{
    public class BattleSystem : SystemBase<BattleSystem>
    {
        public List<TeamAgent> agents { get; private set; }
        public bool isRunning { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            agents = new List<TeamAgent>();
        }
        private void Update()
        {
            if (isRunning)
            {
                ORCASystem.Instance.Update();
            }
        }

        #region Game

        public void StartGame()
        {
            isRunning = true;
            SetActiveAgents(true);
        }

        public void StopGame()
        {
            isRunning = false;
            SetActiveAgents(false);
        }

        #endregion

        #region Agent

        public void AddAgent(TeamAgent agent)
        {
            agents.Add(agent);
        }

        public TeamAgent GetAgent(int id)
        {
            return agents.First(o => o.id == id);
        }

        public void SetActiveAgents(bool isActive)
        {
            foreach (var item in agents)
            {
                if (isActive)
                {
                    item.OnEnable();
                }
                else
                {
                    item.OnDisable();
                }
            }
        }

        #endregion

        #region GameUnit

        private Dictionary<int, Transform> unitGroupDic = new Dictionary<int, Transform>();

        public void CreateUnit<TCtrl>(GameUnitName unitName, Vector3 worldPos, int agentId) where TCtrl : GameUnitCtrl
        {
            TCtrl ctrl = SpawnSystem.Instance.CreateCtrl<TCtrl>(unitName);
            ctrl.transform.position = worldPos;
            SetAgentForUnit(ctrl, agentId);
        }

        public void SetAgentForUnit(GameUnitCtrl ctrl, int agentId)
        {
            TeamAgent agent = GetAgent(agentId);
            ctrl.SetAgent(agent);
            if (!unitGroupDic.TryGetValue(agentId, out Transform group))
            {
                group = new GameObject("Agent Group " + agent.id).transform;
                unitGroupDic[agentId] = group;
            }
            ctrl.transform.SetParent(group);
        }
        #endregion
    }
}