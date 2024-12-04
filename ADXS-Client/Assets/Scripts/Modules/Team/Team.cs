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
        public List<GameUnitCtrl> selectedUnits { get; set; }

        public Transform testCube;

        private void Awake()
        {
            Test();
        }

        private void OnEnable()
        {
            control.OpenControl();
        }

        private void OnDisable()
        {
            control.CloseControl();
        }

        private void Test()
        {
            SetControlWay(new KeyboardCommand(this));
        }


        public void SetControlWay(ITeamControl _control)
        {
            control = _control;
        }


    }
}
