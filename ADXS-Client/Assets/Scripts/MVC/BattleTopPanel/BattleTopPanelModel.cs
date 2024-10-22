using Assets.Scripts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Top
{
    public class BattleTopPanelModel
    {
        public Image sunImg { get; private set; }
        public Image moonImg { get; private set; }


        public BattleTopPanelModel()
        {
            LoadImg();

        }

        public void LoadImg()
        {
            if (sunImg == null)
            {

            }
            if (moonImg == null)
            {

            }
        }
    }
}
