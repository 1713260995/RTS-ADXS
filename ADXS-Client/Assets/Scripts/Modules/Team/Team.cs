using Assets.Scripts.Modules.Team.Control;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.Team
{
    public class Team : MonoBehaviour
    {
        public int id { get; set; }

        public ITeamControl control { get; set; }

        public List<Transform> teamUnitList;


        private void Awake()
        {
            control = new KeyboardCommand(teamUnitList, o =>
            {
                o.ForEach(t =>
                {
                    Debug.Log("多选到单位：" + t.name);
                });
            });
        }

        private void OnEnable()
        {
            control.OpenControl();
        }

        private void OnDisable()
        {
            control.CloseControl();
        }

    }
}
