using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity
{
    public class PlayerAsset
    {
        // 资源属性
        public int gold { get; private set; }
        public int wood { get; private set; }
        public int population { get; private set; }

        public void RefreshGameResources()
        {

        }

        public void SetDefault()
        {
            gold = 500;
            wood = 150;
            population = 5;
        }
    }

}
