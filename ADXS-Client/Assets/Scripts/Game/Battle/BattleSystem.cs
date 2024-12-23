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

    public class BattleSystem : MonoBehaviour //: SingletonMono<BattleSystem>
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


        [ShowButton]
        public void StartGame()
        {
            if (isRuning) return;
            isRuning = true;
            foreach (var item in agents)
            {
                item.OnEnable();
            }
        }

        [ShowButton]
        public void StopGame()
        {
            if (!isRuning) return;
            isRuning = false;
            foreach (var item in agents)
            {
                item.OnDisable();
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

        public void CreateGameUnit<TCtrl>(GameUnitName unitName, int agentId, Vector3 worldPos) where TCtrl : GameUnitCtrl
        {
            TCtrl ctrl = spawnSystem.CreateCtrl<TCtrl>(unitName);
            Agent agent = GetAgent(agentId);
            ctrl.SetAgent(agent);
            ctrl.transform.position = worldPos;
        }

        #endregion
    }
}
