using UnityEngine;

namespace Assets.Scripts.Modules.Team
{
    public class Team : MonoBehaviour
    {
        public int id { get; set; }

        public ITeamControl command { get; set; }

        private void Start()
        {
            command = new KeyboardCommand();
        }


        private void Update()
        {
            //    command.Command();
        }
    }
}
