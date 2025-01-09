using System;

namespace Assets.Scripts.Entity
{
    public class Room
    {
        public string id { get; set; }

        public string homeowner { get; set; }


        public Room()
        {
            id = Guid.NewGuid().ToString();
        }
    }

    public class PlayerInfo
    {
        public int id { get; set; }

        //阵营
        public int teamId { get; set; }


    }
}
