using Assets.GameClientLib.Scripts.Utils.Singleton;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Modules
{
    public class GameUnitManager : Singleton<GameUnitManager>
    {
        public List<GameUnitCtrl> allGameUnits { get; set; } = new List<GameUnitCtrl>();

    }
}
