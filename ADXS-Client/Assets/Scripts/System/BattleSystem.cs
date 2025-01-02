using Assets.GameClientLib.Scripts.Utils.Singleton;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Spawn;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using GameClientLib.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Modules.Battle
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField]
        private SpawnSystem spawnSystem;

        public List<TeamAgent> agents { get; private set; }
        public bool isRunning { get; private set; }
        public static BattleSystem Instance;

        private void Awake() {
            Instance = this;
            agents = new List<TeamAgent>();
        }

        private void Start() { }

        private void Update() {
            if (isRunning) {
                ORCASystem.Instance.Update();
            }
        }

        public void StartGame() {
            isRunning = true;
            SetActiveAgents(true);
        }

        public void StopGame() {
            isRunning = false;
            SetActiveAgents(false);
        }

        public void SetActiveAgents(bool isActive) {
            foreach (var item in agents) {
                if (isActive) {
                    item.OnEnable();
                }
                else {
                    item.OnDisable();
                }
            }
        }

        #region Agent

        public void AddAgent(TeamAgent agent) {
            agents.Add(agent);
        }

        public TeamAgent GetAgent(int id) {
            return agents.First(o => o.id == id);
        }

        #endregion


        #region GameUnit

        private Dictionary<int, Transform> unitGroupDic = new Dictionary<int, Transform>();

        public void CreateGameUnit<TCtrl>(GameUnitName unitName, int agentId, Vector3 worldPos) where TCtrl : GameUnitCtrl {
            TCtrl ctrl = spawnSystem.CreateCtrl<TCtrl>(unitName);
            TeamAgent agent = GetAgent(agentId);
            ctrl.SetAgent(agent);
            if (!unitGroupDic.TryGetValue(agentId, out Transform group)) {
                group = new GameObject("Agent Group " + agent.id).transform;
                unitGroupDic[agentId] = group;
            }

            ctrl.transform.SetParent(group);
            ctrl.transform.position = worldPos;
        }

        #endregion
    }
}