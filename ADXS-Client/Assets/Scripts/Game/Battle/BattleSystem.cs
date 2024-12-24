using Assets.GameClientLib.Scripts.Utils.Singleton;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Spawn;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.Battle
{

    public class BattleSystem : MonoBehaviour
    {
        [SerializeField]
        private SpawnSystem spawnSystem;

        public List<Agent> agents { get; private set; }
        public bool isRuning { get; private set; }
        public static BattleSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            agents = new List<Agent>();
        }

        public void StartGame()
        {
            SetActiveAgents(true);
        }

        public void StopGame()
        {
            SetActiveAgents(false);
        }


        public void SetActiveAgents(bool isActivite)
        {
            foreach (var item in agents)
            {
                if (isActivite)
                {
                    item.OnEnable();
                }
                else
                {
                    item.OnDisable();
                }
            }
        }


        #region Agent

        public void AddAgent(Agent agent)
        {
            agents.Add(agent);
        }

        public Agent GetAgent(int id)
        {
            return agents.First(o => o.id == id);
        }

        #endregion


        #region GameUnit

        private Dictionary<int, Transform> unitGroupDic = new Dictionary<int, Transform>();

        public void CreateGameUnit<TCtrl>(GameUnitName unitName, int agentId, Vector3 worldPos) where TCtrl : GameUnitCtrl
        {
            TCtrl ctrl = spawnSystem.CreateCtrl<TCtrl>(unitName);
            Agent agent = GetAgent(agentId);
            ctrl.SetAgent(agent);
            if (!unitGroupDic.TryGetValue(agentId, out Transform group))
            {
                group = new GameObject("Agent Group " + agent.id).transform;
                unitGroupDic[agentId] = group;
            }
            ctrl.transform.SetParent(group);
            ctrl.transform.position = worldPos;

        }

        #endregion
    }
}
