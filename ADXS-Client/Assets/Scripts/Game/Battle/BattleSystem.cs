using Assets.GameClientLib.Scripts.Utils.Singleton;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Spawn;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.Battle
{

    public class BattleSystem : SingletonMono<BattleSystem>
    {
        [SerializeField]
        private SpawnSystem spawnSystem;

        public List<Agent> teams { get; private set; }

        private void Start()
        {
            teams = new List<Agent>();
        }



        public void StartGame()
        {

        }

        #region Agents
        public void AddAgent(Agent agent)
        {
            teams.Add(agent);
        }

        public Agent GetAgent(int id)
        {
            return teams.First(o => o.id == id);
        }
        #endregion


        #region GameUnit
        public void CreateGameUnit<TCtrl>(GameUnitName unitName, int agentId, Vector3 worldPos) where TCtrl : GameUnitCtrl
        {
            TCtrl ctrl = spawnSystem.CreateCtrl<TCtrl>(unitName);
            Agent agent = GetAgent(agentId);
            ctrl.Init(agent);
            ctrl.transform.position = worldPos;
        }

        #endregion
    }
}
