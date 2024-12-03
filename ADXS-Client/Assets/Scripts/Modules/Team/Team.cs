using Assets.Scripts.Modules.Team.Control;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.Team
{
    public class Team : MonoBehaviour
    {
        public int id { get; set; }

        private ITeamControl control { get; set; }

        public Transform testCube;

        private void Awake()
        {

            SetControlWay(new KeyboardCommand(this));
        }

        private void OnEnable()
        {
            control.OpenControl();
        }

        private void OnDisable()
        {
            control.CloseControl();
        }


        public void SetControlWay(ITeamControl _control)
        {
            control = _control;
        }


    }
}
